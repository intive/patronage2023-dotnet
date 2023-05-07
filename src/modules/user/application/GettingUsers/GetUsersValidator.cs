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
		this.RuleFor(budget => budget.PageIndex).GreaterThan(0);
		this.RuleFor(budget => budget.PageSize).GreaterThan(0);
	}
}