using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using School.Application.AcademicYears.Create;
using School.Application.AcademicYears.GetAll;
using School.Application.AcademicYears.GetById;
using School.Application.Administration.AcademicYears;
using School.Application.Administration.Courses;
using School.Application.Administration.Lessons;
using School.Application.Administration.Semesters;
using School.Application.Administration.Subjects;
using School.Application.Courses.Create;
using School.Application.Courses.GetAll;
using School.Application.Courses.GetById;
using School.Application.Identity.Login;
using School.Application.Identity.Logout;
using School.Application.Identity.RefreshToken;
using School.Application.Identity.Register;
using School.Application.Lessons.Create;
using School.Application.Lessons.GetAll;
using School.Application.Lessons.GetById;
using School.Application.Progress.GetLesson;
using School.Application.Progress.GetMyProgress;
using School.Application.Progress.Update;
using School.Application.Semesters.Create;
using School.Application.Semesters.GetAll;
using School.Application.Semesters.GetById;
using School.Application.Subjects.Create;
using School.Application.Subjects.GetAll;
using School.Application.Subjects.GetById;
using School.Application.Videos.Delete;
using School.Application.Videos.GetById;
using School.Application.Videos.Upload;
using School.Domain.AcademicYears;
using School.Domain.Courses;
using School.Domain.Identity;
using School.Domain.Lessons;
using School.Domain.Progress;
using School.Domain.Semesters;
using School.Domain.Subjects;
using School.Domain.Users;
using School.Domain.Videos;
using School.Infrastructure.Identity;
using School.Infrastructure.Identity.Jwt;
using School.Infrastructure.Persistence;
using School.Infrastructure.Persistence.Repositories;
using School.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration["DATABASE_CONNECTION_STRING"]
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Database connection string is not configured. Set DATABASE_CONNECTION_STRING or ConnectionStrings__DefaultConnection.");
}

builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// JWT settings - prefer explicit env vars (uppercase) or section-based overrides
var jwtSecret =
    builder.Configuration["JWT_SECRET"]
    ?? builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("JWT_SECRET is required.");

var jwtIssuer =
    builder.Configuration["JWT_ISSUER"]
    ?? builder.Configuration["Jwt:Issuer"]
    ?? "SchoolPlatform";

var jwtAudience =
    builder.Configuration["JWT_AUDIENCE"]
    ?? builder.Configuration["Jwt:Audience"]
    ?? "SchoolPlatform";

builder.Services.Configure<JwtSettings>(options =>
{
    options.Secret = jwtSecret;
    options.Issuer = jwtIssuer;
    options.Audience = jwtAudience;
    options.AccessTokenExpirationMinutes = int.TryParse(
        builder.Configuration["JWT_ACCESS_TOKEN_EXPIRATION_MINUTES"],
        out var minutes)
        ? minutes
        : 60;
});

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<LogoutService>();

// Courses
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<CreateCourseService>();
builder.Services.AddScoped<GetCoursesService>();
builder.Services.AddScoped<GetCourseService>();
builder.Services.AddScoped<UpdateCourseService>();
builder.Services.AddScoped<DeleteCourseService>();

// AcademicYears
builder.Services.AddScoped<IAcademicYearRepository, AcademicYearRepository>();
builder.Services.AddScoped<CreateAcademicYearService>();
builder.Services.AddScoped<GetAcademicYearsService>();
builder.Services.AddScoped<GetAcademicYearService>();
builder.Services.AddScoped<UpdateAcademicYearService>();
builder.Services.AddScoped<DeleteAcademicYearService>();

// Semesters
builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<CreateSemesterService>();
builder.Services.AddScoped<GetSemestersService>();
builder.Services.AddScoped<GetSemesterService>();
builder.Services.AddScoped<UpdateSemesterService>();
builder.Services.AddScoped<DeleteSemesterService>();

// Subjects
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<CreateSubjectService>();
builder.Services.AddScoped<GetSubjectsService>();
builder.Services.AddScoped<GetSubjectService>();
builder.Services.AddScoped<UpdateSubjectService>();
builder.Services.AddScoped<DeleteSubjectService>();

// Lessons
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<CreateLessonService>();
builder.Services.AddScoped<GetLessonsService>();
builder.Services.AddScoped<GetLessonService>();
builder.Services.AddScoped<UpdateLessonService>();
builder.Services.AddScoped<DeleteLessonService>();

// Videos
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<GetVideoService>();
builder.Services.AddScoped<UploadVideoService>();
builder.Services.AddScoped<DeleteVideoService>();

var storageEndpoint = builder.Configuration["STORAGE_ENDPOINT"];
var storageAccessKey = builder.Configuration["STORAGE_ACCESS_KEY"];
var storageSecretKey = builder.Configuration["STORAGE_SECRET_KEY"];
var storageBucket = builder.Configuration["STORAGE_BUCKET"];

if (!string.IsNullOrWhiteSpace(storageEndpoint)
    && !string.IsNullOrWhiteSpace(storageAccessKey)
    && !string.IsNullOrWhiteSpace(storageSecretKey)
    && !string.IsNullOrWhiteSpace(storageBucket))
{
    // Object Storage compatible with S3 (e.g. Cloudflare R2, MinIO)
    builder.Services.AddSingleton<IObjectStorage>(_ =>
        new S3ObjectStorage(
            storageEndpoint,
            storageAccessKey,
            storageSecretKey,
            storageBucket));
}
else
{
    // Local file storage (development fallback)
    builder.Services.AddSingleton<IObjectStorage>(_ =>
        new ObjectStorage(
            builder.Configuration["Storage:BasePath"] ?? "uploads"));
}

// Progress
builder.Services.AddScoped<ILessonProgressRepository, LessonProgressRepository>();
builder.Services.AddScoped<UpdateProgressService>();
builder.Services.AddScoped<GetLessonProgressService>();
builder.Services.AddScoped<GetMyProgressService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSecret)
        ),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<SchoolDbContext>("database");

var corsOrigins = builder.Configuration["CORS_ORIGINS"];
var allowedOrigins = string.IsNullOrWhiteSpace(corsOrigins)
    ? new[] { "http://localhost:3000", "http://localhost:3001" }
    : corsOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Seed test users (only if no users exist)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    
    if (!db.Users.Any())
    {
        db.Users.Add(new User("Admin Test", "admin@school.com", hasher.Hash("admin123"), UserRole.Admin));
        db.Users.Add(new User("Student Test", "student@school.com", hasher.Hash("student123"), UserRole.Student));
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();