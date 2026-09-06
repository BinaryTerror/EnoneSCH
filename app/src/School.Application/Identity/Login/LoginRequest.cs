namespace School.Application.Identity.Login;

public record LoginRequest(
    string Email,
    string Password
);
