using School.Domain.Progress;

namespace School.Application.Progress.GetMyProgress;

public class GetMyProgressService
{
    private readonly ILessonProgressRepository _repository;

    public GetMyProgressService(ILessonProgressRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<LessonProgress>> GetMyProgressAsync(Guid studentId)
    {
        return await _repository.GetByStudentAsync(studentId);
    }
}