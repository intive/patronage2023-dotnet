using FluentValidation;
using Intive.Patronage2023.Api.User.CreatingUser;
using Intive.Patronage2023.Modules.Example.Application.Example;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intive.Patronage2023.Modules.Example.Api.Controllers;

/// <summary>
/// Sign up Controller.
/// </summary>
[ApiController]
[Route("[controller]")]
public class SignUpController : ControllerBase
{
	private readonly IQueryBus queryBus;
	private readonly IValidator<CreateUser> createUserValidator;

	/// <summary>
	/// Initializes a new instance of the <see cref="SignUpController"/> class.
	/// </summary>
	/// <param name="queryBus">command bus.</param>
	/// <param name="createUserValidator">create user validator.</param>
	public SignUpController(IQueryBus queryBus, IValidator<CreateUser> createUserValidator)
	{
		this.queryBus = queryBus;
		this.createUserValidator = createUserValidator;
	}

	/// <summary>
	/// Creates user.
	/// </summary>
	/// <param name="query">Command.</param>
	/// <returns>Created command.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     {
	///        "avatar": "some Avatar.",
	///        "firstName": "Jan",
	///        "lastName": "Kowalski",
	///        "password": "testPasword123!@",
	///        "email": "jkowalski@gmail.com"
	///     }
	/// .</remarks>
	/// <response code="200">Indicates if the request to create user was done correctly.</response>
	/// <response code="400">If the body is not valid.</response>
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[AllowAnonymous]
	[HttpPost]
	public async Task<IActionResult> SignUp([FromBody] CreateUser query)
	{
		var validationResult = await this.createUserValidator.ValidateAsync(query);
		if (validationResult.IsValid)
		{
			await this.queryBus.Query<CreateUser, HttpResponseMessage>(query);
			return this.Ok();
		}

		throw new AppException("One or more error occured while trying to create user.", validationResult.Errors);
	}
}