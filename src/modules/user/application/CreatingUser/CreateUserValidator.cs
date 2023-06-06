using FluentValidation;

namespace Intive.Patronage2023.Modules.User.Application.CreatingUser;

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
		this.RuleFor(user => user.Avatar).NotEmpty().WithErrorCode("3.1");
		this.RuleFor(user => user.FirstName).NotEmpty().WithErrorCode("3.2");
		this.RuleFor(user => user.LastName).NotEmpty().WithErrorCode("3.3");
		this.RuleFor(user => user.Email).EmailAddress().WithErrorCode("3.5").NotEmpty().WithErrorCode("3.4");

		this.RuleFor(user => user.Password)
			.MinimumLength(12).WithErrorCode("3.6")
			.Must(password => password.Any(char.IsUpper)).WithErrorCode("3.6")
			.Must(password => password.Any(char.IsLower)).WithErrorCode("3.6")
			.Must(password => password.Any(char.IsDigit)).WithErrorCode("3.6")
			.Must(password => !password.Contains(" ")).WithErrorCode("3.6")
			.Matches(@"[!\""#$%&'()+,\-./:;<=>?@\[\]*^_`{|}~\\]+").WithErrorCode("3.6")
			.NotEmpty().WithErrorCode("3.6");
	}
}