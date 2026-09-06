using School.Domain.Progress;
using School.Domain.Subjects;
using School.Domain.Videos;

namespace School.Domain.Lessons;

public class Lesson
{
    public Guid Id { get; private set; }
    public Guid SubjectId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int Position { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public Subject Subject { get; private set; } = null!;
    public ICollection<Video> Videos { get; private set; } = [];
    public ICollection<LessonProgress> Progresses { get; private set; } = [];

    private Lesson() { }

    public Lesson(Guid subjectId, string title, string description, int position)
    {
        Id = Guid.NewGuid();
        SubjectId = subjectId;
        Title = title;
        Description = description;
        Position = position;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description, int position)
    {
        Title = title;
        Description = description;
        Position = position;
        UpdatedAt = DateTime.UtcNow;
    }
}