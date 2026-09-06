using School.Domain.Courses;

namespace School.Application.Courses.GetById;

public class GetCourseService
{
    private readonly ICourseRepository _repository;

    public GetCourseService(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Course?> GetByIdAsync(Guid courseId)
    {
        return await _repository.GetByIdAsync(courseId);
    }
}