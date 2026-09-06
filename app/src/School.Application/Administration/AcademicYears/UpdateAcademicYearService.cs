using School.Domain.AcademicYears;

namespace School.Application.Administration.AcademicYears;

public class UpdateAcademicYearService
{
    private readonly IAcademicYearRepository _repository;

    public UpdateAcademicYearService(IAcademicYearRepository repository)
    {
        _repository = repository;
    }

    public async Task UpdateAsync(Guid yearId, int yearNumber)
    {
        var academicYear = await _repository.GetByIdAsync(yearId)
            ?? throw new InvalidOperationException("Academic year not found.");

        academicYear.Update(yearNumber);
        await _repository.UpdateAsync(academicYear);
    }
}