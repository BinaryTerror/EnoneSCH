using School.Domain.Semesters;

namespace School.Application.Semesters.Create;

public class CreateSemesterService
{
    private readonly ISemesterRepository _repository;

    public CreateSemesterService(ISemesterRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(Guid academicYearId, CreateSemesterRequest request)
    {
        var semester = new Semester(
            academicYearId,
            request.SemesterNumber
        );

        await _repository.CreateAsync(semester);

        return semester.Id;
    }
}