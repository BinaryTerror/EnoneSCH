using School.Domain.Subjects;

namespace School.Application.Subjects.Create;

public class CreateSubjectService
{
    private readonly ISubjectRepository _repository;

    public CreateSubjectService(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(Guid semesterId, CreateSubjectRequest request)
    {
        var subject = new Subject(
            semesterId,
            request.Name.Trim(),
            request.Description.Trim()
        );

        await _repository.CreateAsync(subject);

        return subject.Id;
    }
}