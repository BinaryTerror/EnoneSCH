using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using School.Domain.Identity;
using School.Infrastructure.Identity.Jwt;

namespace School.Infrastructure.Identity;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenGenerator(IOptions<JwtSettings> options)
    {
        _jwtSettings = options.Value;
    }

    public string GenerateToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public string HashToken(string token)
    {
        var key = _jwtSettings.Secret;
        var combined = Encoding.UTF8.GetBytes(token + key);

        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(combined);
        return Convert.ToBase64String(hash);
    }
}
