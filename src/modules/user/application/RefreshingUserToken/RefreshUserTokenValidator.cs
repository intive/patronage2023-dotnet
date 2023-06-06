using FluentValidation;

namespace Intive.Patronage2023.Modules.User.Application.RefreshingUserToken;

/// <summary>
/// Refresh user token validator.
/// </summary>
public class RefreshUserTokenValidator : AbstractValidator<RefreshUserToken>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="RefreshUserTokenValidator"/> class.
	/// </summary>
	public RefreshUserTokenValidator()
	{
		this.RuleFor(x => x.RefreshToken).NotEmpty().NotNull();
	}
}