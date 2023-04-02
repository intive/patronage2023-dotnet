using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intive.Patronage2023.Modules.Example.Api.Controllers;

/// <summary>
/// TestController.
/// </summary>
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class TestController : ControllerBase
{
	/// <summary>
	/// Returns ok if authorized.
	/// </summary>
	/// <returns>Paged list of examples.</returns>
	/// <response code="200">Returns 200Ok if authorized.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public IActionResult ReturnOkIfAuthorized()
	{
		return this.Ok();
	}
}