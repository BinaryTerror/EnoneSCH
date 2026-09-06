using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Semesters.GetAll;
using School.Application.Semesters.GetById;

namespace School.Api.Controllers;

[ApiController]
[Route("api/semesters")]
public class SemestersController : ControllerBase
{
    private readonly GetSemestersService _getAllService;
    private readonly GetSemesterService _getByIdService;

    public SemestersController(
        GetSemestersService getAllService,
        GetSemesterService getByIdService)
    {
        _getAllService = getAllService;
        _getByIdService = getByIdService;
    }

    [HttpGet("{semesterId}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid semesterId)
    {
        var semester = await _getByIdService.GetByIdAsync(semesterId);
        if (semester is null)
            return NotFound(new { message = "Semester not found." });
        return Ok(semester);
    }
}

[ApiController]
[Route("api/academic-years/{academicYearId}/semesters")]
public class AcademicYearSemestersController : ControllerBase
{
    private readonly GetSemestersService _getAllService;

    public AcademicYearSemestersController(GetSemestersService getAllService)
    {
        _getAllService = getAllService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(Guid academicYearId)
    {
        var semesters = await _getAllService.GetAllAsync(academicYearId);
        return Ok(semesters);
    }
}