using Microsoft.EntityFrameworkCore;
using School.Domain.Identity;
using School.Domain.Users;
using School.Infrastructure.Persistence;

namespace School.Application.Identity.Register;

public class RegisterService
{
    private readonly SchoolDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterService(
        SchoolDbContext dbContext,
        IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> RegisterAsync(
        RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var exists = await _dbContext.Users
            .AnyAsync(x => x.Email == email);

        if (exists)
        {
            throw new InvalidOperationException(
                "Email already registered."
            );
        }

        var passwordHash = _passwordHasher.Hash(
            request.Password
        );

        var user = new User(
            request.FullName.Trim(),
            email,
            passwordHash,
            UserRole.Student
        );

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();

        return user.Id;
    }
}
