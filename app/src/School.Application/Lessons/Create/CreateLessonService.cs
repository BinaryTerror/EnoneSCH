using School.Domain.Lessons;

namespace School.Application.Lessons.Create;

public class CreateLessonService
{
    private readonly ILessonRepository _repository;

    public CreateLessonService(ILessonRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(Guid subjectId, CreateLessonRequest request)
    {
        var lesson = new Lesson(
            subjectId,
            request.Title.Trim(),
            request.Description.Trim(),
            request.Position
        );

        await _repository.CreateAsync(lesson);

        return lesson.Id;
    }
}