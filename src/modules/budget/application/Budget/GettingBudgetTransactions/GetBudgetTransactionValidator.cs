using FluentValidation;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;

/// <summary>
/// GetBudgetsValidator class.
/// </summary>
public class GetBudgetTransactionValidator : AbstractValidator<GetBudgetTransaction>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetTransactionValidator"/> class.
	/// </summary>
	public GetBudgetTransactionValidator()
	{
		this.RuleFor(budget => budget.PageIndex).GreaterThan(0);
		this.RuleFor(budget => budget.PageSize).GreaterThan(0);
	}
}