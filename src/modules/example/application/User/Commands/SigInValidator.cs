using FluentValidation;

namespace Intive.Patronage2023.Modules.Example.Application.User.Commands;

/// <summary>
/// SigIn validator class.
/// </summary>
public class SigInValidator : AbstractValidator<SignInCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SigInValidator"/> class.
	/// </summary>
	public SigInValidator()
	{
		this.RuleFor(signin => signin.Username).NotEmpty().NotNull();
		this.RuleFor(signin => signin.Password).NotEmpty().NotNull();
	}
}