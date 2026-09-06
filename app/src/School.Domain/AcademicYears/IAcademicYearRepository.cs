namespace School.Domain.AcademicYears;

public interface IAcademicYearRepository
{
    Task<AcademicYear?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<AcademicYear>> GetAllAsync(Guid? courseId = null);
    Task CreateAsync(AcademicYear academicYear);
    Task UpdateAsync(AcademicYear academicYear);
    Task DeleteAsync(Guid id);
}