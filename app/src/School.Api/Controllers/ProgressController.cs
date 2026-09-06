using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Progress.GetLesson;
using School.Application.Progress.GetMyProgress;
using School.Application.Progress.Update;

namespace School.Api.Controllers;

[ApiController]
[Route("api/progress")]
[Authorize]
public class ProgressController : ControllerBase
{
    private readonly UpdateProgressService _updateService;
    private readonly GetLessonProgressService _getLessonService;
    private readonly GetMyProgressService _getMyProgressService;

    public ProgressController(
        UpdateProgressService updateService,
        GetLessonProgressService getLessonService,
        GetMyProgressService getMyProgressService)
    {
        _updateService = updateService;
        _getLessonService = getLessonService;
        _getMyProgressService = getMyProgressService;
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProgressRequest request)
    {
        var studentId = GetCurrentUserId();
        var progressId = await _updateService.UpdateAsync(studentId, request);
        return Ok(new { id = progressId });
    }

    [HttpGet("lesson/{lessonId}")]
    public async Task<IActionResult> GetByLesson(Guid lessonId)
    {
        var studentId = GetCurrentUserId();
        var progress = await _getLessonService.GetByLessonAsync(studentId, lessonId);

        if (progress is null)
        {
            return NotFound(new { message = "No progress found for this lesson." });
        }

        return Ok(progress);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProgress()
    {
        var studentId = GetCurrentUserId();
        var progress = await _getMyProgressService.GetMyProgressAsync(studentId);
        return Ok(progress);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
}