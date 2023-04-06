using System.Net;
using FluentValidation;
using Intive.Patronage2023.Modules.Example.Application.Example;
using Intive.Patronage2023.Modules.Example.Application.User.Commands;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Modules.Example.Api.Controllers;

/// <summary>
/// text.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
	private readonly ICommandBus commandBus;
	private readonly IQueryBus queryBus;
	private readonly IValidator<SignInUser> signInUserValidator;

	/// <summary>
	/// Initializes a new instance of the <see cref="AuthenticationController"/> class.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="signInCommandValidator">SignIn User validator.</param>
	public AuthenticationController(ICommandBus commandBus, IQueryBus queryBus, IValidator<SignInUser> signInCommandValidator)
	{
		this.signInUserValidator = signInCommandValidator;
		this.commandBus = commandBus;
		this.queryBus = queryBus;
	}

	/// <summary>
	/// text.
	/// </summary>
	/// <param name="command">User login and password.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Successfully signed in.</response>
	/// <response code="401">Username or password is not valid.</response>
	/// <response code="500">Internal server error.</response>
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
	[AllowAnonymous]
	[HttpPost("SignIn")]
	public async Task<IActionResult> SignInUserAsync([FromBody] SignInUser command)
	{
		var validationResult = await this.signInUserValidator.ValidateAsync(command);
		if (validationResult.IsValid)
		{
			HttpResponseMessage response = await this.queryBus.Query<SignInUser, HttpResponseMessage>(command);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				return this.Unauthorized();
			}

			string responseContent = await response.Content.ReadAsStringAsync();
			if (responseContent != null)
			{
				Token? token = JsonConvert.DeserializeObject<Token>(responseContent);
				return this.Ok(token);
			}
		}

		throw new AppException("One or more error occured when trying to get token.", validationResult.Errors);
	}

	/// <summary>
	/// [Test] Returns ok if authorized.
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