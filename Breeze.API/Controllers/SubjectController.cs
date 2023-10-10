using Breeze.Services.Subject;
using Microsoft.AspNetCore.Mvc;

namespace Breeze.API.Controllers;
public class SubjectController : BaseApiController
{
    private readonly ISubjectFacadeService _subjectFacadeService;

    public SubjectController(ISubjectFacadeService subjectFacadeService)
    {
        _subjectFacadeService = subjectFacadeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSubjectSummary(int subjectId)
        => Ok(await _subjectFacadeService.GetSubjectSummary(subjectId));
}