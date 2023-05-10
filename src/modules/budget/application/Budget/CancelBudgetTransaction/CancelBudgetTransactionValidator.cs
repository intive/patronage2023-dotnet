using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CancellBudgetTransaction;

/// <summary>
/// Cancel Budget validator class.
/// </summary>
public class CancelBudgetTransactionValidator : AbstractValidator<CancelBudgetTransaction>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="CancelBudgetTransactionValidator"/> class.
	/// </summary>
	/// <param name="budgetDbContext">BudgetDbContext.</param>
	public CancelBudgetTransactionValidator(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;

		this.RuleFor(transaction => transaction.Id)
			.NotEmpty()
			.NotNull();

		this.RuleFor(transaction => transaction.Id)
			.MustAsync(async (x, cancellation) => await this.IsTransactionExists(x));
	}

	private async Task<bool> IsTransactionExists(Guid id)
	{
		return await this.budgetDbContext.Transaction.AnyAsync(b => b.Id.Equals(new TransactionId(id)));
	}
}