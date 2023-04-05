using FluentValidation;
using Intive.Patronage2023.Modules.Example.Api.User.CreatingUser;
using Intive.Patronage2023.Modules.Example.Application.Example;
using Intive.Patronage2023.Shared.Abstractions.Commands;
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
	private readonly ICommandBus commandBus;
	private readonly IValidator<CreateUser> createUserValidator;

	/// <summary>
	/// Initializes a new instance of the <see cref="SignUpController"/> class.
	/// </summary>
	/// <param name="commandBus">command bus.</param>
	/// <param name="createUserValidator">create user validator.</param>
	public SignUpController(ICommandBus commandBus, IValidator<CreateUser> createUserValidator)
	{
		this.commandBus = commandBus;
		this.createUserValidator = createUserValidator;
	}

	/// <summary>
	/// Creates user.
	/// </summary>
	/// <param name="command">Command.</param>
	/// <returns>Created command.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     {
	///        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	///        "FirstName": "Jan",
	///        "LastName": "Kowalski",
	///        "Password": "testPasword123!@",
	///        "Email": "jkowalski@gmail.com"
	///     }
	/// .</remarks>
	/// <response code="201">Returns the newly created user.</response>
	/// <response code="400">If the body is not valid.</response>
	[ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	[AllowAnonymous]
	[HttpPost]
	public async Task<IActionResult> SignUp([FromBody] CreateUser command)
	{
		var validationResult = await this.createUserValidator.ValidateAsync(command);
		if (validationResult.IsValid)
		{
			await this.commandBus.Send(command);
			return this.Created($"user/{command.Id}", command.Id);
		}

		throw new AppException("One or more error occured while trying to create user.", validationResult.Errors);
	}
}