using School.Domain.AcademicYears;

namespace School.Application.AcademicYears.GetAll;

public class GetAcademicYearsService
{
    private readonly IAcademicYearRepository _repository;

    public GetAcademicYearsService(IAcademicYearRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<AcademicYear>> GetAllAsync(Guid? courseId = null)
    {
        return await _repository.GetAllAsync(courseId);
    }
}