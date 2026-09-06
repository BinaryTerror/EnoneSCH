using School.Domain.Semesters;

namespace School.Application.Semesters.GetById;

public class GetSemesterService
{
    private readonly ISemesterRepository _repository;

    public GetSemesterService(ISemesterRepository repository)
    {
        _repository = repository;
    }

    public async Task<Semester?> GetByIdAsync(Guid semesterId)
    {
        return await _repository.GetByIdAsync(semesterId);
    }
}