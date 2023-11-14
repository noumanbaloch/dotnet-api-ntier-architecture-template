using Breeze.Services.Subject;
using Microsoft.AspNetCore.Mvc;

namespace Breeze.API.Controllers;
public class SubjectController(ISubjectFacadeService _subjectFacadeService) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetSubjectSummary(int subjectId)
        => Ok(await _subjectFacadeService.GetSubjectSummary(subjectId));
}