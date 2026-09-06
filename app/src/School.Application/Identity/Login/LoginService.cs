using Microsoft.EntityFrameworkCore;
using School.Application.Identity.RefreshToken;
using School.Domain.Identity;
using School.Domain.Users;
using School.Infrastructure.Persistence;

namespace School.Application.Identity.Login;

public class LoginService
{
    private readonly SchoolDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly RefreshTokenService _refreshTokenService;

    public LoginService(
        SchoolDbContext dbContext,
        IPasswordHasher passwordHasher,
        RefreshTokenService refreshTokenService)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<LoginResponse> LoginAsync(
        LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user is null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password."
            );
        }

        var isValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash
        );

        if (!isValid)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password."
            );
        }

        var (accessToken, refreshToken, expiresAt) =
            await _refreshTokenService.CreateTokensAsync(user);

        return new LoginResponse(
            accessToken,
            refreshToken,
            expiresAt,
            new UserDto(
                user.Id,
                user.FullName,
                user.Email,
                user.Role.ToString()
            )
        );
    }
}
