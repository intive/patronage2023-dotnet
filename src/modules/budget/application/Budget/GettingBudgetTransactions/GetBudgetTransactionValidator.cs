using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingTransaction;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;

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
	}
}