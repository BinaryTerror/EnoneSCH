using School.Domain.Users;

namespace School.Domain.Identity;

public interface IJwtTokenGenerator
{
    (string AccessToken, DateTime ExpiresAt) Generate(User user);
}
