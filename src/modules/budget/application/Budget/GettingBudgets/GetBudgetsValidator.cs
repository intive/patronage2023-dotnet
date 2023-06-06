using FluentValidation;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;

/// <summary>
/// GetBudgetsValidator class.
/// </summary>
public class GetBudgetsValidator : AbstractValidator<GetBudgets>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetsValidator"/> class.
	/// </summary>
	public GetBudgetsValidator()
	{
		this.RuleFor(budget => budget.PageIndex).GreaterThan(0).WithErrorCode("10.1");
		this.RuleFor(budget => budget.PageSize).GreaterThan(0).WithErrorCode("10.1");
	}
}