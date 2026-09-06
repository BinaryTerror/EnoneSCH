using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Subjects.Create;
using School.Application.Subjects.GetById;
using School.Application.Subjects.GetAll;
using School.Application.Administration.Subjects;

namespace School.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/subjects")]
[Authorize(Roles = "Admin")]
public class SubjectsController : ControllerBase
{
    private readonly CreateSubjectService _createService;
    private readonly GetSubjectService _getByIdService;
    private readonly GetSubjectsService _getAllService;
    private readonly UpdateSubjectService _updateService;
    private readonly DeleteSubjectService _deleteService;

    public SubjectsController(
        CreateSubjectService createService,
        GetSubjectService getByIdService,
        GetSubjectsService getAllService,
        UpdateSubjectService updateService,
        DeleteSubjectService deleteService)
    {
        _createService = createService;
        _getByIdService = getByIdService;
        _getAllService = getAllService;
        _updateService = updateService;
        _deleteService = deleteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? semesterId)
    {
        var subjects = await _getAllService.GetAllAsync(semesterId);
        return Ok(subjects);
    }

    [HttpGet("{subjectId}")]
    public async Task<IActionResult> GetById(Guid subjectId)
    {
        var subject = await _getByIdService.GetByIdAsync(subjectId);
        if (subject is null)
            return NotFound(new { message = "Subject not found." });
        return Ok(subject);
    }

    [HttpPost("{semesterId}")]
    public async Task<IActionResult> Create(Guid semesterId, CreateSubjectRequest request)
    {
        var subjectId = await _createService.CreateAsync(semesterId, request);
        return Created($"/api/admin/subjects/{subjectId}", new { id = subjectId });
    }

    [HttpPut("{subjectId}")]
    public async Task<IActionResult> Update(Guid subjectId, CreateSubjectRequest request)
    {
        try
        {
            await _updateService.UpdateAsync(subjectId, request.Name, request.Description);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{subjectId}")]
    public async Task<IActionResult> Delete(Guid subjectId)
    {
        await _deleteService.DeleteAsync(subjectId);
        return NoContent();
    }
}