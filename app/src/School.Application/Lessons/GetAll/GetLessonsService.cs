using School.Domain.Lessons;

namespace School.Application.Lessons.GetAll;

public class GetLessonsService
{
    private readonly ILessonRepository _repository;

    public GetLessonsService(ILessonRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<Lesson>> GetAllAsync(Guid? subjectId = null)
    {
        return await _repository.GetAllAsync(subjectId);
    }
}