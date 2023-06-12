using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CancelBudgetTransaction;

/// <summary>
/// Cancel Budget Transaction Command.
/// </summary>
/// <param name="TransactionId">Id of Transaction to be deleted.</param>
/// <param name="BudgetId">Id of Budget for which transaction will be deleted.</param>
public record CancelBudgetTransaction(Guid TransactionId, Guid BudgetId) : ICommand;

/// <summary>
/// Cancel Budget Transaction Command Handler.
/// </summary>
public class HandleCancelBudgetTransaction : ICommandHandler<CancelBudgetTransaction>
{
	private readonly IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleCancelBudgetTransaction"/> class.
	/// </summary>
	/// <param name="budgetTransactionRepository">Repository that manages Budget Transaction aggregate root.</param>
	public HandleCancelBudgetTransaction(IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository)
	{
		this.budgetTransactionRepository = budgetTransactionRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(CancelBudgetTransaction command, CancellationToken cancellationToken)
	{
		var transactionId = new TransactionId(command.TransactionId);
		var transaction = await this.budgetTransactionRepository.GetById(transactionId);
		transaction!.CancelTransaction();
		await this.budgetTransactionRepository.Persist(transaction);
	}
}