using FluentValidation;

namespace Intive.Patronage2023.Modules.User.Application.GettingUsers;

/// <summary>
/// GetUsers validator class.
/// </summary>
public class GetUsersValidator : AbstractValidator<GetUsers>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GetUsersValidator"/> class.
	/// </summary>
	public GetUsersValidator()
	{
		this.RuleFor(user => user.PageIndex).GreaterThan(0).WithErrorCode("10.1");
		this.RuleFor(user => user.PageSize).GreaterThan(0).WithErrorCode("10.1");
	}
}