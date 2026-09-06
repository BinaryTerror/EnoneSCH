namespace School.Domain.Lessons;

public interface ILessonRepository
{
    Task<Lesson?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Lesson>> GetAllAsync(Guid? subjectId = null);
    Task CreateAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task DeleteAsync(Guid id);
}