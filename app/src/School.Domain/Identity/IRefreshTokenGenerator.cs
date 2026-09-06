namespace School.Domain.Identity;

public interface IRefreshTokenGenerator
{
    string GenerateToken();

    string HashToken(string token);
}
