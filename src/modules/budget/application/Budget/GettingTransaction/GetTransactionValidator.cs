using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingTransaction;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;

/// <summary>
/// GetBudgetsValidator class.
/// </summary>
public class GetTransactionValidator : AbstractValidator<GetTransaction>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GetTransactionValidator"/> class.
	/// </summary>
	public GetTransactionValidator()
	{
		this.RuleFor(x => x.BudgetId).NotNull().NotEmpty();
	}
}