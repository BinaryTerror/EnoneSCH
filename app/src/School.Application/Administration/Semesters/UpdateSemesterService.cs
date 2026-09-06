using School.Domain.Semesters;

namespace School.Application.Administration.Semesters;

public class UpdateSemesterService
{
    private readonly ISemesterRepository _repository;

    public UpdateSemesterService(ISemesterRepository repository)
    {
        _repository = repository;
    }

    public async Task UpdateAsync(Guid semesterId, int semesterNumber)
    {
        var semester = await _repository.GetByIdAsync(semesterId)
            ?? throw new InvalidOperationException("Semester not found.");

        semester.Update(semesterNumber);
        await _repository.UpdateAsync(semester);
    }
}