using FluentValidation;
using Intive.Patronage2023.Modules.Example.Application.Example;
using Intive.Patronage2023.Modules.Example.Application.User.CreatingUser;
using Intive.Patronage2023.Shared.Abstractions.Commands;
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
	/// Creates user. Username length has to be in range (6,30). Emails needs to be in valid address format.
	/// </summary>
	/// <param name="command">Command.</param>
	/// <returns>Created command.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     {
	///        "username": "admin1",
	///        "password": "admin1",
	///        "email": "admin1@gmail.com"
	///     }
	/// .</remarks>
	/// <response code="201">Returns the newly created item.</response>
	/// <response code="400">If the body is not valid.</response>
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<IActionResult> SignUp([FromBody] CreateUser command)
	{
		var validationResult = await this.createUserValidator.ValidateAsync(command);
		if (validationResult.IsValid)
		{
			await this.commandBus.Send(command);
			return this.Created($"user/{command.Id}", command.Id);
		}

		throw new AppException("One or more error occured when trying to create user.", validationResult.Errors);
	}
}