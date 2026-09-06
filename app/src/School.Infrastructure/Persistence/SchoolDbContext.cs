using Microsoft.EntityFrameworkCore;
using School.Domain.AcademicYears;
using School.Domain.Courses;
using School.Domain.Identity;
using School.Domain.Lessons;
using School.Domain.Progress;
using School.Domain.Semesters;
using School.Domain.Subjects;
using School.Domain.Users;
using School.Domain.Videos;

namespace School.Infrastructure.Persistence;

public class SchoolDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();
    public DbSet<Semester> Semesters => Set<Semester>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<LessonProgress> LessonProgresses => Set<LessonProgress>();

    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchoolDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}