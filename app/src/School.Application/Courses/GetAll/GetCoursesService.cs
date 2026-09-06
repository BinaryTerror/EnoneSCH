using School.Domain.Courses;

namespace School.Application.Courses.GetAll;

public class GetCoursesService
{
    private readonly ICourseRepository _repository;

    public GetCoursesService(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<Course>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
}