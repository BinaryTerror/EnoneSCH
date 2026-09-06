namespace School.Application.Progress.Update;

public record UpdateProgressRequest(
    Guid LessonId,
    int WatchedSeconds,
    bool Completed
);