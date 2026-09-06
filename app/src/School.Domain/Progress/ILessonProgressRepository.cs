namespace School.Domain.Progress;

public interface ILessonProgressRepository
{
    Task<LessonProgress?> GetByIdAsync(Guid id);
    Task<LessonProgress?> GetByStudentAndLessonAsync(Guid studentId, Guid lessonId);
    Task<IReadOnlyList<LessonProgress>> GetByStudentAsync(Guid studentId);
    Task<IReadOnlyList<LessonProgress>> GetByLessonAsync(Guid lessonId);
    Task CreateAsync(LessonProgress progress);
    Task UpdateAsync(LessonProgress progress);
}