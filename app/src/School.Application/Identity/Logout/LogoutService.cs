using Microsoft.EntityFrameworkCore;
using School.Domain.Identity;
using School.Infrastructure.Persistence;

namespace School.Application.Identity.Logout;

public class LogoutService
{
    private readonly SchoolDbContext _dbContext;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public LogoutService(
        SchoolDbContext dbContext,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _dbContext = dbContext;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var tokenHash = _refreshTokenGenerator.HashToken(refreshToken);

        var token = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);

        if (token is not null && token.IsActive)
        {
            token.Revoke();
            await _dbContext.SaveChangesAsync();
        }
    }
}