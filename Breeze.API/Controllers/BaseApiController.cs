using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Breeze.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = UserRoles.STUDENT_ROLE)]
public class BaseApiController : ControllerBase
{

}
