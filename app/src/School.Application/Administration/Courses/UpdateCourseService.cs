using School.Domain.Courses;

namespace School.Application.Administration.Courses;

public class UpdateCourseService
{
    private readonly ICourseRepository _repository;

    public UpdateCourseService(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task UpdateAsync(Guid courseId, string name, string description)
    {
        var course = await _repository.GetByIdAsync(courseId);
        if (course is null)
            throw new InvalidOperationException("Course not found.");

        course.Update(name.Trim(), description.Trim());
        await _repository.UpdateAsync(course);
    }
}