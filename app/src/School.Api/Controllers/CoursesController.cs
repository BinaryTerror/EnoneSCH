using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Courses.Create;
using School.Application.Courses.GetAll;
using School.Application.Courses.GetById;
using School.Application.Administration.Courses;
using School.Domain.Courses;

namespace School.Api.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly CreateCourseService _createService;
    private readonly GetCoursesService _getAllService;
    private readonly GetCourseService _getByIdService;
    private readonly UpdateCourseService _updateService;
    private readonly DeleteCourseService _deleteService;

    public CoursesController(
        CreateCourseService createService,
        GetCoursesService getAllService,
        GetCourseService getByIdService,
        UpdateCourseService updateService,
        DeleteCourseService deleteService)
    {
        _createService = createService;
        _getAllService = getAllService;
        _getByIdService = getByIdService;
        _updateService = updateService;
        _deleteService = deleteService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _getAllService.GetAllAsync();
        return Ok(courses);
    }

    [HttpGet("{courseId}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid courseId)
    {
        var course = await _getByIdService.GetByIdAsync(courseId);

        if (course is null)
        {
            return NotFound(new { message = "Course not found." });
        }

        return Ok(course);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateCourseRequest request)
    {
        var courseId = await _createService.CreateAsync(request);
        return Created($"/api/courses/{courseId}", new { id = courseId });
    }

    [HttpPut("{courseId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid courseId, CreateCourseRequest request)
    {
        try
        {
            await _updateService.UpdateAsync(courseId, request.Name, request.Description);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{courseId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid courseId)
    {
        await _deleteService.DeleteAsync(courseId);
        return NoContent();
    }
}