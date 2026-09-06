using School.Domain.Progress;

namespace School.Application.Progress.GetLesson;

public class GetLessonProgressService
{
    private readonly ILessonProgressRepository _repository;

    public GetLessonProgressService(ILessonProgressRepository repository)
    {
        _repository = repository;
    }

    public async Task<LessonProgress?> GetByLessonAsync(Guid studentId, Guid lessonId)
    {
        return await _repository.GetByStudentAndLessonAsync(studentId, lessonId);
    }
}