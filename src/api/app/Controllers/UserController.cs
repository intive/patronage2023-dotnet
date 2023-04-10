using System.Net;
using FluentValidation;
using Intive.Patronage2023.Api.User;
using Intive.Patronage2023.Modules.Example.Application.Example;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Api;

/// <summary>
/// User Controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly IQueryBus queryBus;
	private readonly IValidator<SignInUser> signInUserValidator;

	/// <summary>
	/// Initializes a new instance of the <see cref="UserController"/> class.
	/// </summary>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="signInCommandValidator">SignIn User validator.</param>
	public UserController(IQueryBus queryBus, IValidator<SignInUser> signInCommandValidator)
	{
		this.signInUserValidator = signInCommandValidator;
		this.queryBus = queryBus;
	}

	/// <summary>
	/// Authenticates a user and returns a JWT token.
	/// </summary>
	/// <param name="command">The sign-in command, which includes the user email and password.</param>
	/// <returns>
	/// An HTTP response containing a JWT token if the sign-in was successful, or an error response
	/// if the sign-in failed.
	/// </returns>
	/// <exception cref="AppException">
	/// Thrown if one or more errors occur while trying to authenticate the user.
	/// </exception>
	/// <response code="200">Return Token class object.</response>
	/// <response code="401">Email or password is not valid.</response>
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
			if (!string.IsNullOrEmpty(responseContent))
			{
				Token? token = JsonConvert.DeserializeObject<Token>(responseContent);
				return this.Ok(token);
			}
		}

		throw new AppException("One or more error occured when trying to get token.", validationResult.Errors);
	}
}