using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Subjects.GetAll;
using School.Application.Subjects.GetById;

namespace School.Api.Controllers;

[ApiController]
[Route("api/subjects")]
public class SubjectsController : ControllerBase
{
    private readonly GetSubjectsService _getAllService;
    private readonly GetSubjectService _getByIdService;

    public SubjectsController(
        GetSubjectsService getAllService,
        GetSubjectService getByIdService)
    {
        _getAllService = getAllService;
        _getByIdService = getByIdService;
    }

    [HttpGet("{subjectId}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid subjectId)
    {
        var subject = await _getByIdService.GetByIdAsync(subjectId);
        if (subject is null)
            return NotFound(new { message = "Subject not found." });
        return Ok(subject);
    }
}

[ApiController]
[Route("api/semesters/{semesterId}/subjects")]
public class SemesterSubjectsController : ControllerBase
{
    private readonly GetSubjectsService _getAllService;

    public SemesterSubjectsController(GetSubjectsService getAllService)
    {
        _getAllService = getAllService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(Guid semesterId)
    {
        var subjects = await _getAllService.GetAllAsync(semesterId);
        return Ok(subjects);
    }
}