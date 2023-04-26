using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;

/// <summary>
/// GetBudgetsValidator class.
/// </summary>
public class GetBudgetTransactionValidator : AbstractValidator<GetBudgetTransaction>
{
	private readonly IBudgetRepository budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetTransactionValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">budgetRepository, so we can validate BudgetId.</param>
	public GetBudgetTransactionValidator(IBudgetRepository budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.RuleFor(budget => budget.PageIndex).GreaterThan(0);
		this.RuleFor(budget => budget.PageSize).GreaterThan(0);
		this.RuleFor(budget => budget.BudgetId).MustAsync(this.IsBudgetExists).NotEmpty().NotNull();
	}

	private async Task<bool> IsBudgetExists(BudgetId budgetGuid, CancellationToken cancellationToken)
	{
		var budget = await this.budgetRepository.GetById(budgetGuid);
		if (budget == null)
		{
			return false;
		}

		return true;
	}
}