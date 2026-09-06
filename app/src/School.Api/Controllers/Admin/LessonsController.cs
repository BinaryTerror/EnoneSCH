using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Lessons.Create;
using School.Application.Lessons.GetById;
using School.Application.Lessons.GetAll;
using School.Application.Administration.Lessons;

namespace School.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/lessons")]
[Authorize(Roles = "Admin,Teacher")]
public class LessonsController : ControllerBase
{
    private readonly CreateLessonService _createService;
    private readonly GetLessonService _getByIdService;
    private readonly GetLessonsService _getAllService;
    private readonly UpdateLessonService _updateService;
    private readonly DeleteLessonService _deleteService;

    public LessonsController(
        CreateLessonService createService,
        GetLessonService getByIdService,
        GetLessonsService getAllService,
        UpdateLessonService updateService,
        DeleteLessonService deleteService)
    {
        _createService = createService;
        _getByIdService = getByIdService;
        _getAllService = getAllService;
        _updateService = updateService;
        _deleteService = deleteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? subjectId)
    {
        var lessons = await _getAllService.GetAllAsync(subjectId);
        return Ok(lessons);
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetById(Guid lessonId)
    {
        var lesson = await _getByIdService.GetByIdAsync(lessonId);
        if (lesson is null)
            return NotFound(new { message = "Lesson not found." });
        return Ok(lesson);
    }

    [HttpPost("{subjectId}")]
    public async Task<IActionResult> Create(Guid subjectId, CreateLessonRequest request)
    {
        var lessonId = await _createService.CreateAsync(subjectId, request);
        return Created($"/api/admin/lessons/{lessonId}", new { id = lessonId });
    }

    [HttpPut("{lessonId}")]
    public async Task<IActionResult> Update(Guid lessonId, CreateLessonRequest request)
    {
        try
        {
            await _updateService.UpdateAsync(lessonId, request.Title, request.Description, request.Position);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{lessonId}")]
    public async Task<IActionResult> Delete(Guid lessonId)
    {
        await _deleteService.DeleteAsync(lessonId);
        return NoContent();
    }
}