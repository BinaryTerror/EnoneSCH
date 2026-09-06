using School.Domain.AcademicYears;
using School.Domain.Subjects;

namespace School.Domain.Semesters;

public class Semester
{
    public Guid Id { get; private set; }
    public Guid AcademicYearId { get; private set; }
    public int SemesterNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public AcademicYear AcademicYear { get; private set; } = null!;
    public ICollection<Subject> Subjects { get; private set; } = [];

    private Semester() { }

    public Semester(Guid academicYearId, int semesterNumber)
    {
        Id = Guid.NewGuid();
        AcademicYearId = academicYearId;
        SemesterNumber = semesterNumber;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(int semesterNumber)
    {
        SemesterNumber = semesterNumber;
        UpdatedAt = DateTime.UtcNow;
    }
}