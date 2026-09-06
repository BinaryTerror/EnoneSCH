using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Lessons.GetAll;
using School.Application.Lessons.GetById;
using School.Application.Videos.GetById;

namespace School.Api.Controllers;

[ApiController]
[Route("api/subjects/{subjectId}/lessons")]
public class SubjectLessonsController : ControllerBase
{
    private readonly GetLessonsService _getAllService;

    public SubjectLessonsController(GetLessonsService getAllService)
    {
        _getAllService = getAllService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(Guid subjectId)
    {
        var lessons = await _getAllService.GetAllAsync(subjectId);
        return Ok(lessons);
    }
}

[ApiController]
[Route("api/lessons")]
public class LessonsController : ControllerBase
{
    private readonly GetLessonService _getByIdService;
    private readonly GetVideoService _getVideoService;

    public LessonsController(
        GetLessonService getByIdService,
        GetVideoService getVideoService)
    {
        _getByIdService = getByIdService;
        _getVideoService = getVideoService;
    }

    [HttpGet("{lessonId}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid lessonId)
    {
        var lesson = await _getByIdService.GetByIdAsync(lessonId);

        if (lesson is null)
        {
            return NotFound(new { message = "Lesson not found." });
        }

        return Ok(lesson);
    }

    [HttpGet("{lessonId}/video")]
    [Authorize]
    public async Task<IActionResult> GetVideoByLesson(Guid lessonId)
    {
        var video = await _getVideoService.GetByLessonIdAsync(lessonId);

        if (video is null)
        {
            return NotFound(new { message = "No video available for this lesson." });
        }

        return Ok(new
        {
            id = video.Id,
            status = video.Status.ToString(),
            durationSeconds = video.DurationSeconds,
            thumbnailUrl = video.ThumbnailUrl,
            streamUrl = $"/api/videos/{video.Id}/stream"
        });
    }
}