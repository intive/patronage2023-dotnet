using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CancelBudgetTransaction;

/// <summary>
/// Cancel Budget validator class.
/// </summary>
public class CancelBudgetTransactionValidator : AbstractValidator<CancelBudgetTransaction>
{
	private readonly IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="CancelBudgetTransactionValidator"/> class.
	/// </summary>
	/// <param name="budgetTransactionRepository">Budget Transaction Repository.</param>
	public CancelBudgetTransactionValidator(IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository)
	{
		this.budgetTransactionRepository = budgetTransactionRepository;

		this.RuleFor(transaction => transaction.BudgetId)
			.NotEmpty()
			.NotNull();

		this.RuleFor(transaction => transaction.TransactionId)
			.NotEmpty()
			.NotNull();

		this.RuleFor(transaction => transaction)
			.MustAsync(async (x, cancellation) => await this.IsTransactionBelongingToBudget(x.BudgetId, x.TransactionId));
	}

	private async Task<bool> IsTransactionBelongingToBudget(Guid budgetId, Guid transactionId)
	{
		var transaction = await this.budgetTransactionRepository.GetById(new TransactionId(transactionId));
		return budgetId == transaction!.BudgetId.Value;
	}
}