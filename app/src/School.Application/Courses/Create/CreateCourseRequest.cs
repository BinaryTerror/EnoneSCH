namespace School.Application.Courses.Create;

public record CreateCourseRequest(
    string Name,
    string Description,
    string? ImageUrl = null
);