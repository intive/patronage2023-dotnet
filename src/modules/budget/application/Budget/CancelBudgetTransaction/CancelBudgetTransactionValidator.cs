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
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="CancelBudgetTransactionValidator"/> class.
	/// </summary>
	/// <param name="budgetTransactionRepository">Budget Transaction Repository.</param>
	/// <param name="budgetRepository">Budget Repository.</param>
	public CancelBudgetTransactionValidator(IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository, IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetTransactionRepository = budgetTransactionRepository;
		this.budgetRepository = budgetRepository;

		this.RuleFor(transaction => transaction.BudgetId)
			.NotEmpty()
			.NotNull();

		this.RuleFor(transaction => transaction.TransactionId)
			.NotEmpty()
			.NotNull();

		this.RuleFor(transaction => transaction.TransactionId)
			.MustAsync(async (x, cancellation) => await this.TransactionExists(x))
			.WithMessage("Transaction doesn't exist.");

		this.RuleFor(transaction => transaction.BudgetId)
			.MustAsync(async (x, cancellation) => await this.BudgetExists(x))
			.WithMessage("Budget doesn't exist.");

		this.RuleFor(transaction => transaction)
			.MustAsync(async (x, cancellation) => await this.BelongsToBudget(x.BudgetId, x.TransactionId))
			.WithMessage("This transaction does not belong to the specified budget.");
	}

	private async Task<bool> TransactionExists(Guid transactionId)
	{
		BudgetTransactionAggregate? transaction = await this.budgetTransactionRepository.GetById(new TransactionId(transactionId));
		return transaction != null;
	}

	private async Task<bool> BudgetExists(Guid budgetId)
	{
		BudgetAggregate? budget = await this.budgetRepository.GetById(new BudgetId(budgetId));
		return budget != null;
	}

	private async Task<bool> BelongsToBudget(Guid budgetId, Guid transactionId)
	{
		var transaction = await this.budgetTransactionRepository.GetById(new TransactionId(transactionId));
		return budgetId == transaction!.BudgetId.Value;
	}
}