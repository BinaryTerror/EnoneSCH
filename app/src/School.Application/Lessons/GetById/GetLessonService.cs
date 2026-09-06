using School.Domain.Lessons;

namespace School.Application.Lessons.GetById;

public class GetLessonService
{
    private readonly ILessonRepository _repository;

    public GetLessonService(ILessonRepository repository)
    {
        _repository = repository;
    }

    public async Task<Lesson?> GetByIdAsync(Guid lessonId)
    {
        return await _repository.GetByIdAsync(lessonId);
    }
}