using FluentValidation;

namespace Intive.Patronage2023.Modules.Example.Application.User.CreatingUser;

/// <summary>
/// Example validator class.
/// </summary>
public class CreateUserValidator : AbstractValidator<CreateUser>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CreateUserValidator"/> class.
	/// </summary>
	public CreateUserValidator()
	{
		this.RuleFor(user => user.Id).NotEmpty().NotNull();
		this.RuleFor(user => user.Username).NotEmpty().NotNull().Length(6, 30);
		this.RuleFor(user => user.Email).NotEmpty().EmailAddress();
		this.RuleFor(user => user.Password).NotEmpty().NotNull();
	}
}