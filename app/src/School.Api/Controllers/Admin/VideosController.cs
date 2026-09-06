using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Videos.GetById;
using School.Application.Videos.Delete;
using School.Application.Videos.Upload;

namespace School.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/videos")]
[Authorize(Roles = "Admin,Teacher")]
public class VideosController : ControllerBase
{
    private readonly GetVideoService _getVideoService;
    private readonly UploadVideoService _uploadService;
    private readonly DeleteVideoService _deleteService;

    public VideosController(
        GetVideoService getVideoService,
        UploadVideoService uploadService,
        DeleteVideoService deleteService)
    {
        _getVideoService = getVideoService;
        _uploadService = uploadService;
        _deleteService = deleteService;
    }

    [HttpGet("{videoId}")]
    public async Task<IActionResult> GetById(Guid videoId)
    {
        var video = await _getVideoService.GetByIdAsync(videoId);
        if (video is null)
            return NotFound(new { message = "Video not found." });
        return Ok(video);
    }

    [HttpPost("{lessonId}")]
    public async Task<IActionResult> Upload(Guid lessonId, IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded." });

        var request = new UploadVideoRequest(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType);

        var videoId = await _uploadService.UploadAsync(lessonId, request);
        return Created($"/api/admin/videos/{videoId}", new { id = videoId });
    }

    [HttpDelete("{videoId}")]
    public async Task<IActionResult> Delete(Guid videoId)
    {
        await _deleteService.DeleteAsync(videoId);
        return NoContent();
    }
}