using School.Domain.Lessons;
using School.Domain.Users;

namespace School.Domain.Progress;

public class LessonProgress
{
    public Guid Id { get; private set; }
    public Guid StudentId { get; private set; }
    public Guid LessonId { get; private set; }
    public int WatchedSeconds { get; private set; }
    public bool Completed { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public User Student { get; private set; } = null!;
    public Lesson Lesson { get; private set; } = null!;

    private LessonProgress() { }

    public LessonProgress(Guid studentId, Guid lessonId, int watchedSeconds, bool completed)
    {
        Id = Guid.NewGuid();
        StudentId = studentId;
        LessonId = lessonId;
        WatchedSeconds = watchedSeconds;
        Completed = completed;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProgress(int watchedSeconds, bool completed)
    {
        WatchedSeconds = watchedSeconds;
        Completed = completed;
        UpdatedAt = DateTime.UtcNow;
    }
}