using School.Domain.Subjects;

namespace School.Application.Subjects.GetById;

public class GetSubjectService
{
    private readonly ISubjectRepository _repository;

    public GetSubjectService(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Subject?> GetByIdAsync(Guid subjectId)
    {
        return await _repository.GetByIdAsync(subjectId);
    }
}