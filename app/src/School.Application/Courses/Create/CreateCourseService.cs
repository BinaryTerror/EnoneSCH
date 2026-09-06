using School.Domain.Courses;

namespace School.Application.Courses.Create;

public class CreateCourseService
{
    private readonly ICourseRepository _repository;

    public CreateCourseService(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(CreateCourseRequest request)
    {
        var course = new Course(
            request.Name.Trim(),
            request.Description.Trim(),
            request.ImageUrl
        );

        await _repository.CreateAsync(course);

        return course.Id;
    }
}