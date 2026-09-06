namespace School.Domain.Subjects;

public interface ISubjectRepository
{
    Task<Subject?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Subject>> GetAllAsync(Guid? semesterId = null);
    Task CreateAsync(Subject subject);
    Task UpdateAsync(Subject subject);
    Task DeleteAsync(Guid id);
}