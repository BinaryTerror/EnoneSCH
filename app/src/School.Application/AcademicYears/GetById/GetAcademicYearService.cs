using School.Domain.AcademicYears;

namespace School.Application.AcademicYears.GetById;

public class GetAcademicYearService
{
    private readonly IAcademicYearRepository _repository;

    public GetAcademicYearService(IAcademicYearRepository repository)
    {
        _repository = repository;
    }

    public async Task<AcademicYear?> GetByIdAsync(Guid yearId)
    {
        return await _repository.GetByIdAsync(yearId);
    }
}