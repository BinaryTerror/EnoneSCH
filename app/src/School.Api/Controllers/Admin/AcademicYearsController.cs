using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.AcademicYears.Create;
using School.Application.AcademicYears.GetById;
using School.Application.AcademicYears.GetAll;
using School.Application.Administration.AcademicYears;

namespace School.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/academic-years")]
[Authorize(Roles = "Admin")]
public class AcademicYearsController : ControllerBase
{
    private readonly CreateAcademicYearService _createService;
    private readonly GetAcademicYearService _getByIdService;
    private readonly GetAcademicYearsService _getAllService;
    private readonly UpdateAcademicYearService _updateService;
    private readonly DeleteAcademicYearService _deleteService;

    public AcademicYearsController(
        CreateAcademicYearService createService,
        GetAcademicYearService getByIdService,
        GetAcademicYearsService getAllService,
        UpdateAcademicYearService updateService,
        DeleteAcademicYearService deleteService)
    {
        _createService = createService;
        _getByIdService = getByIdService;
        _getAllService = getAllService;
        _updateService = updateService;
        _deleteService = deleteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? courseId)
    {
        var years = await _getAllService.GetAllAsync(courseId);
        return Ok(years);
    }

    [HttpGet("{yearId}")]
    public async Task<IActionResult> GetById(Guid yearId)
    {
        var year = await _getByIdService.GetByIdAsync(yearId);
        if (year is null)
            return NotFound(new { message = "Academic year not found." });
        return Ok(year);
    }

    [HttpPost("{courseId}")]
    public async Task<IActionResult> Create(Guid courseId, CreateAcademicYearRequest request)
    {
        var yearId = await _createService.CreateAsync(courseId, request);
        return Created($"/api/admin/academic-years/{yearId}", new { id = yearId });
    }

    [HttpPut("{yearId}")]
    public async Task<IActionResult> Update(Guid yearId, CreateAcademicYearRequest request)
    {
        try
        {
            await _updateService.UpdateAsync(yearId, request.YearNumber);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{yearId}")]
    public async Task<IActionResult> Delete(Guid yearId)
    {
        await _deleteService.DeleteAsync(yearId);
        return NoContent();
    }
}