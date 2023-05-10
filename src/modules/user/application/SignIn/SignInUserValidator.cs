using FluentValidation;

namespace Intive.Patronage2023.Modules.User.Application.SignIn;

/// <summary>
/// SigIn validator class.
/// </summary>
public class SignInUserValidator : AbstractValidator<SignInUser>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SignInUserValidator"/> class.
	/// </summary>
	public SignInUserValidator()
	{
		this.RuleFor(signin => signin.Email).NotEmpty().NotNull();
		this.RuleFor(signin => signin.Password).NotEmpty().NotNull();
	}
}