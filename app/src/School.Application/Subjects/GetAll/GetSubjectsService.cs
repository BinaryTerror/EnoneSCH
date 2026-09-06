using School.Domain.Subjects;

namespace School.Application.Subjects.GetAll;

public class GetSubjectsService
{
    private readonly ISubjectRepository _repository;

    public GetSubjectsService(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<Subject>> GetAllAsync(Guid? semesterId = null)
    {
        return await _repository.GetAllAsync(semesterId);
    }
}