using School.Domain.Progress;

namespace School.Application.Progress.Update;

public class UpdateProgressService
{
    private readonly ILessonProgressRepository _repository;

    public UpdateProgressService(ILessonProgressRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> UpdateAsync(Guid studentId, UpdateProgressRequest request)
    {
        var existing = await _repository.GetByStudentAndLessonAsync(
            studentId,
            request.LessonId);

        if (existing is not null)
        {
            existing.UpdateProgress(request.WatchedSeconds, request.Completed);
            await _repository.UpdateAsync(existing);
            return existing.Id;
        }

        var progress = new LessonProgress(
            studentId,
            request.LessonId,
            request.WatchedSeconds,
            request.Completed);

        await _repository.CreateAsync(progress);

        return progress.Id;
    }
}