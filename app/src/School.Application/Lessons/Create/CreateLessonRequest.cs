namespace School.Application.Lessons.Create;

public record CreateLessonRequest(
    string Title,
    string Description,
    int Position
);