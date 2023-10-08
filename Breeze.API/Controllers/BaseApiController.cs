using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Breeze.Models.Constants;

namespace Breeze.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = UserRoles.ADMIN_ROLE)]
public class BaseApiController : ControllerBase
{

}
