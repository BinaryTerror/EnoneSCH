using School.Domain.Lessons;
using School.Domain.Semesters;

namespace School.Domain.Subjects;

public class Subject
{
    public Guid Id { get; private set; }
    public Guid SemesterId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public Semester Semester { get; private set; } = null!;
    public ICollection<Lesson> Lessons { get; private set; } = [];

    private Subject() { }

    public Subject(Guid semesterId, string name, string description, string? imageUrl = null)
    {
        Id = Guid.NewGuid();
        SemesterId = semesterId;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description, string? imageUrl = null)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}