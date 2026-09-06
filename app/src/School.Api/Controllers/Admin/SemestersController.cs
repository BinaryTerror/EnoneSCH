using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Administration.Semesters;
using School.Application.Semesters.Create;
using School.Application.Semesters.GetById;
using School.Application.Semesters.GetAll;

namespace School.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/semesters")]
[Authorize(Roles = "Admin")]
public class SemestersController : ControllerBase
{
    private readonly CreateSemesterService _createService;
    private readonly GetSemesterService _getByIdService;
    private readonly GetSemestersService _getAllService;
    private readonly UpdateSemesterService _updateService;
    private readonly DeleteSemesterService _deleteService;

    public SemestersController(
        CreateSemesterService createService,
        GetSemesterService getByIdService,
        GetSemestersService getAllService,
        UpdateSemesterService updateService,
        DeleteSemesterService deleteService)
    {
        _createService = createService;
        _getByIdService = getByIdService;
        _getAllService = getAllService;
        _updateService = updateService;
        _deleteService = deleteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? academicYearId)
    {
        var semesters = await _getAllService.GetAllAsync(academicYearId);
        return Ok(semesters);
    }

    [HttpGet("{semesterId}")]
    public async Task<IActionResult> GetById(Guid semesterId)
    {
        var semester = await _getByIdService.GetByIdAsync(semesterId);
        if (semester is null)
            return NotFound(new { message = "Semester not found." });
        return Ok(semester);
    }

    [HttpPost("{academicYearId}")]
    public async Task<IActionResult> Create(Guid academicYearId, CreateSemesterRequest request)
    {
        var semesterId = await _createService.CreateAsync(academicYearId, request);
        return Created($"/api/admin/semesters/{semesterId}", new { id = semesterId });
    }

    [HttpPut("{semesterId}")]
    public async Task<IActionResult> Update(Guid semesterId, CreateSemesterRequest request)
    {
        try
        {
            await _updateService.UpdateAsync(semesterId, request.SemesterNumber);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{semesterId}")]
    public async Task<IActionResult> Delete(Guid semesterId)
    {
        await _deleteService.DeleteAsync(semesterId);
        return NoContent();
    }
}