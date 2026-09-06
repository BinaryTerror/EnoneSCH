using School.Domain.Subjects;

namespace School.Application.Administration.Subjects;

public class UpdateSubjectService
{
    private readonly ISubjectRepository _repository;

    public UpdateSubjectService(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task UpdateAsync(Guid subjectId, string name, string description)
    {
        var subject = await _repository.GetByIdAsync(subjectId);
        if (subject is null)
            throw new InvalidOperationException("Subject not found.");

        subject.Update(name.Trim(), description.Trim());
        await _repository.UpdateAsync(subject);
    }
}