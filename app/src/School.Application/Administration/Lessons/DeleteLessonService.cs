using School.Domain.Lessons;

namespace School.Application.Administration.Lessons;

public class DeleteLessonService
{
    private readonly ILessonRepository _repository;

    public DeleteLessonService(ILessonRepository repository)
    {
        _repository = repository;
    }

    public async Task DeleteAsync(Guid lessonId)
    {
        await _repository.DeleteAsync(lessonId);
    }
}