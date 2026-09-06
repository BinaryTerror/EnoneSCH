using School.Domain.Courses;
using School.Domain.Semesters;

namespace School.Domain.AcademicYears;

public class AcademicYear
{
    public Guid Id { get; private set; }
    public Guid CourseId { get; private set; }
    public int YearNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public Course Course { get; private set; } = null!;
    public ICollection<Semester> Semesters { get; private set; } = [];

    private AcademicYear() { }

    public AcademicYear(Guid courseId, int yearNumber)
    {
        Id = Guid.NewGuid();
        CourseId = courseId;
        YearNumber = yearNumber;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(int yearNumber)
    {
        YearNumber = yearNumber;
        UpdatedAt = DateTime.UtcNow;
    }
}