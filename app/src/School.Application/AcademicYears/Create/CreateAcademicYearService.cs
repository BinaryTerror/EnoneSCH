using School.Domain.AcademicYears;

namespace School.Application.AcademicYears.Create;

public class CreateAcademicYearService
{
    private readonly IAcademicYearRepository _repository;

    public CreateAcademicYearService(IAcademicYearRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(Guid courseId, CreateAcademicYearRequest request)
    {
        var academicYear = new AcademicYear(
            courseId,
            request.YearNumber
        );

        await _repository.CreateAsync(academicYear);

        return academicYear.Id;
    }
}