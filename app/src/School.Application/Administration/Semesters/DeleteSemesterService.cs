using School.Domain.Semesters;

namespace School.Application.Administration.Semesters;

public class DeleteSemesterService
{
    private readonly ISemesterRepository _repository;

    public DeleteSemesterService(ISemesterRepository repository)
    {
        _repository = repository;
    }

    public async Task DeleteAsync(Guid semesterId)
    {
        await _repository.DeleteAsync(semesterId);
    }
}