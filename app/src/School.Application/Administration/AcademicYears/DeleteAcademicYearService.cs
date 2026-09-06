using School.Domain.AcademicYears;

namespace School.Application.Administration.AcademicYears;

public class DeleteAcademicYearService
{
    private readonly IAcademicYearRepository _repository;

    public DeleteAcademicYearService(IAcademicYearRepository repository)
    {
        _repository = repository;
    }

    public async Task DeleteAsync(Guid academicYearId)
    {
        await _repository.DeleteAsync(academicYearId);
    }
}