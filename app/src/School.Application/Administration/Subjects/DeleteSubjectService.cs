using School.Domain.Subjects;

namespace School.Application.Administration.Subjects;

public class DeleteSubjectService
{
    private readonly ISubjectRepository _repository;

    public DeleteSubjectService(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task DeleteAsync(Guid subjectId)
    {
        await _repository.DeleteAsync(subjectId);
    }
}