using School.Domain.Semesters;

namespace School.Application.Semesters.GetAll;

public class GetSemestersService
{
    private readonly ISemesterRepository _repository;

    public GetSemestersService(ISemesterRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<Semester>> GetAllAsync(Guid? academicYearId = null)
    {
        return await _repository.GetAllAsync(academicYearId);
    }
}