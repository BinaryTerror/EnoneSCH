using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.AcademicYears.Create;
using School.Application.AcademicYears.GetAll;
using School.Application.AcademicYears.GetById;
using School.Domain.AcademicYears;

namespace School.Api.Controllers;

[ApiController]
[Route("api/academic-years")]
public class AcademicYearsController : ControllerBase
{
    private readonly CreateAcademicYearService _createService;
    private readonly GetAcademicYearsService _getAllService;
    private readonly GetAcademicYearService _getByIdService;

    public AcademicYearsController(
        CreateAcademicYearService createService,
        GetAcademicYearsService getAllService,
        GetAcademicYearService getByIdService)
    {
        _createService = createService;
        _getAllService = getAllService;
        _getByIdService = getByIdService;
    }

    [HttpGet("{yearId}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid yearId)
    {
        var academicYear = await _getByIdService.GetByIdAsync(yearId);

        if (academicYear is null)
        {
            return NotFound(new { message = "Academic year not found." });
        }

        return Ok(academicYear);
    }

    [HttpPut("{yearId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid yearId, CreateAcademicYearRequest request)
    {
        return Ok(new { message = "Not implemented yet." });
    }

    [HttpDelete("{yearId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid yearId)
    {
        return Ok(new { message = "Not implemented yet." });
    }
}

[ApiController]
[Route("api/courses/{courseId}/years")]
public class CourseAcademicYearsController : ControllerBase
{
    private readonly CreateAcademicYearService _createService;
    private readonly GetAcademicYearsService _getAllService;

    public CourseAcademicYearsController(
        CreateAcademicYearService createService,
        GetAcademicYearsService getAllService)
    {
        _createService = createService;
        _getAllService = getAllService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(Guid courseId)
    {
        var academicYears = await _getAllService.GetAllAsync(courseId);
        return Ok(academicYears);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Guid courseId, CreateAcademicYearRequest request)
    {
        var yearId = await _createService.CreateAsync(courseId, request);
        return Created($"/api/academic-years/{yearId}", new { id = yearId });
    }
}