using School.Domain.Lessons;

namespace School.Application.Administration.Lessons;

public class UpdateLessonService
{
    private readonly ILessonRepository _repository;

    public UpdateLessonService(ILessonRepository repository)
    {
        _repository = repository;
    }

    public async Task UpdateAsync(Guid lessonId, string title, string description, int position)
    {
        var lesson = await _repository.GetByIdAsync(lessonId);
        if (lesson is null)
            throw new InvalidOperationException("Lesson not found.");

        lesson.Update(title.Trim(), description.Trim(), position);
        await _repository.UpdateAsync(lesson);
    }
}