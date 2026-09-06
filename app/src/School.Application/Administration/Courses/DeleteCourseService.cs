using School.Domain.Courses;

namespace School.Application.Administration.Courses;

public class DeleteCourseService
{
    private readonly ICourseRepository _repository;

    public DeleteCourseService(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task DeleteAsync(Guid courseId)
    {
        await _repository.DeleteAsync(courseId);
    }
}