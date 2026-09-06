namespace School.Application.Identity.Register;

public record RegisterRequest(
    string FullName,
    string Email,
    string Password
);
