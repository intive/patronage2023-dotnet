using FluentValidation;
using Intive.Patronage2023.Modules.Example.Application.User.Commands;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intive.Patronage2023.Modules.Example.Api.Controllers;

/// <summary>
/// text.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly ICommandBus commandBus;
	private readonly IValidator<SignInCommand> signInCommandValidator;

	/// <summary>
	/// Initializes a new instance of the <see cref="UserController"/> class.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="signInCommandValidator">SignIn command validator.</param>
	public UserController(ICommandBus commandBus, IValidator<SignInCommand> signInCommandValidator)
	{
		this.signInCommandValidator = signInCommandValidator;
		this.commandBus = commandBus;
	}

	/// <summary>
	/// text.
	/// </summary>
	/// <param name="command">User password.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Successfully signed in.</response>
	/// <response code="401">Username or password is not valid.</response>
	/// <response code="500">Internal server error.</response>
	[HttpPost("SignIn")]
	public async Task<IActionResult> SignInUserAsync([FromBody] SignInCommand command)
	{
		var validationResult = await this.signInCommandValidator.ValidateAsync(command);
		if (validationResult.IsValid)
		{
			var response = await this.commandBus.Send(command);
			return this.Ok(response);
		}

		return this.BadRequest();
	}

	/// <summary>
	/// Returns ok if authorized.
	/// </summary>
	/// <returns>Paged list of examples.</returns>
	/// <response code="200">Returns 200Ok if authorized.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[HttpGet]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public IActionResult ReturnOkIfAuthorized()
	{
		return this.Ok();
	}
}