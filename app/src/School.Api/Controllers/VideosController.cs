using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Videos.Create;
using School.Application.Videos.Delete;
using School.Application.Videos.GetById;
using School.Application.Videos.Upload;
using School.Domain.Videos;
using School.Infrastructure.Storage;

namespace School.Api.Controllers;

[ApiController]
[Route("api/videos")]
public class VideosController : ControllerBase
{
    private readonly GetVideoService _getVideoService;
    private readonly DeleteVideoService _deleteVideoService;
    private readonly IObjectStorage _storage;

    public VideosController(
        GetVideoService getVideoService,
        DeleteVideoService deleteVideoService,
        IObjectStorage storage)
    {
        _getVideoService = getVideoService;
        _deleteVideoService = deleteVideoService;
        _storage = storage;
    }

    [HttpGet("{videoId}/stream")]
    [Authorize]
    public async Task<IActionResult> Stream(Guid videoId)
    {
        var video = await _getVideoService.GetByIdAsync(videoId);

        if (video is null)
        {
            return NotFound(new { message = "Video not found." });
        }

        var stream = await _storage.DownloadAsync(video.StorageKey);
        return File(stream, "video/mp4", enableRangeProcessing: true);
    }

    [HttpGet("{videoId}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid videoId)
    {
        var video = await _getVideoService.GetByIdAsync(videoId);

        if (video is null)
        {
            return NotFound(new { message = "Video not found." });
        }

        return Ok(video);
    }

    [HttpDelete("{videoId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid videoId)
    {
        await _deleteVideoService.DeleteAsync(videoId);
        return NoContent();
    }
}

[ApiController]
[Route("api/lessons/{lessonId}/videos")]
public class LessonVideosController : ControllerBase
{
    private readonly UploadVideoService _uploadService;
    private readonly GetVideoService _getVideoService;

    public LessonVideosController(
        UploadVideoService uploadService,
        GetVideoService getVideoService)
    {
        _uploadService = uploadService;
        _getVideoService = getVideoService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetByLesson(Guid lessonId)
    {
        var video = await _getVideoService.GetByLessonIdAsync(lessonId);

        if (video is null)
        {
            return NotFound(new { message = "No video found for this lesson." });
        }

        return Ok(video);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Upload(
        Guid lessonId,
        IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded." });
        }

        var request = new UploadVideoRequest(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType);

        var videoId = await _uploadService.UploadAsync(lessonId, request);

        return Created($"/api/videos/{videoId}", new { id = videoId });
    }
}