namespace School.Domain.Courses;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Course>> GetAllAsync();
    Task CreateAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(Guid id);
}