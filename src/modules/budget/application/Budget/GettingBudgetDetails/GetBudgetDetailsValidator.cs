using FluentValidation;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;

/// <summary>
/// GetBudgetsValidator class.
/// </summary>
public class GetBudgetDetailsValidator : AbstractValidator<GetBudgetDetails>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetDetailsValidator"/> class.
	/// </summary>
	public GetBudgetDetailsValidator()
	{
		this.RuleFor(budget => budget.Id).NotEmpty();
	}
}