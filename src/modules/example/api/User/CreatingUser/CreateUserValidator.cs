using FluentValidation;

namespace Intive.Patronage2023.Modules.Example.Api.User.CreatingUser;

/// <summary>
/// User validator class.
/// </summary>
public class CreateUserValidator : AbstractValidator<CreateUser>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CreateUserValidator"/> class.
	/// </summary>
	public CreateUserValidator()
	{
		this.RuleFor(user => user.Id).NotEmpty().NotNull();
		this.RuleFor(user => user.FirstName).NotEmpty();
		this.RuleFor(user => user.LastName).NotEmpty();
		this.RuleFor(user => user.Email).EmailAddress();

		this.RuleFor(user => user.Password)
			.MinimumLength(12)
			.Must(password => password.Any(char.IsUpper))
			.Must(password => password.Any(char.IsLower))
			.Must(password => password.Any(char.IsDigit))
			.Must(password => !password.Contains(" "))
			.Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]");
	}
}