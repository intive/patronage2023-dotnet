using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CancellBudgetTransaction;

/// <summary>
/// Cancell Budget validator class.
/// </summary>
public class CancellBudgetTransactionValidator : AbstractValidator<CancellBudgetTransaction>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="CancellBudgetTransactionValidator"/> class.
	/// </summary>
	/// <param name="budgetDbContext">BudgetDbContext.</param>
	public CancellBudgetTransactionValidator(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;

		this.RuleFor(transaction => transaction.Id)
			.NotEmpty()
			.NotNull();

		this.RuleFor(transaction => new { transaction.Id })
			.MustAsync((x, cancellation) => this.IsExistingBudget(x.Id));
	}

	private async Task<bool> IsExistingBudget(Guid id)
	{
		return await this.budgetDbContext.Transaction.AnyAsync(b => b.Id.Equals(new TransactionId(id)));
	}
}