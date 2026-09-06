namespace School.Domain.Semesters;

public interface ISemesterRepository
{
    Task<Semester?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Semester>> GetAllAsync(Guid? academicYearId = null);
    Task CreateAsync(Semester semester);
    Task UpdateAsync(Semester semester);
    Task DeleteAsync(Guid id);
}