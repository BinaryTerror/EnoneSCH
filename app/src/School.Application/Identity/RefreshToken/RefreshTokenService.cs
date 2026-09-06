using Microsoft.EntityFrameworkCore;
using School.Domain.Identity;
using School.Infrastructure.Persistence;

namespace School.Application.Identity.RefreshToken;

public class RefreshTokenService
{
    private readonly SchoolDbContext _dbContext;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public RefreshTokenService(
        SchoolDbContext dbContext,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<RefreshTokenResponse> RefreshAsync(
        RefreshTokenRequest request)
    {
        var tokenHash = _refreshTokenGenerator.HashToken(
            request.RefreshToken
        );

        var refreshToken = await _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);

        if (refreshToken is null)
        {
            throw new UnauthorizedAccessException(
                "Invalid refresh token."
            );
        }

        if (!refreshToken.IsActive)
        {
            throw new UnauthorizedAccessException(
                "Refresh token is expired or revoked."
            );
        }

        if (refreshToken.User is null)
        {
            throw new UnauthorizedAccessException(
                "User not found."
            );
        }

        refreshToken.Revoke();
        await _dbContext.SaveChangesAsync();

        return await CreateTokensAsync(refreshToken.User);
    }

    public async Task<RefreshTokenResponse> CreateTokensAsync(
        Domain.Users.User user)
    {
        var (accessToken, expiresAt) = _jwtTokenGenerator.Generate(user);

        var refreshTokenRaw = _refreshTokenGenerator.GenerateToken();
        var refreshTokenHash = _refreshTokenGenerator.HashToken(refreshTokenRaw);

        var refreshToken = new Domain.Identity.RefreshToken(
            user.Id,
            refreshTokenHash,
            DateTime.UtcNow.AddDays(7)
        );

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        return new RefreshTokenResponse(
            accessToken,
            refreshTokenRaw,
            expiresAt
        );
    }
}
