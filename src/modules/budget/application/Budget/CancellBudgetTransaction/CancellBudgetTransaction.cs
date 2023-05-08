using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CancellBudgetTransaction;

/// <summary>
/// Cancell Budget Transaction Command.
/// </summary>
/// <param name="Id">Budget identifier.</param>
public record CancellBudgetTransaction(Guid Id) : ICommand;

/// <summary>
/// Cancell Budget Transaction Command Handler.
/// </summary>
public class HandleCancellBudgetTransaction : ICommandHandler<CancellBudgetTransaction>
{
	private readonly IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository;
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleCancellBudgetTransaction"/> class.
	/// </summary>
	/// <param name="budgetTransactionRepository">Repository that manages Budget Transaction aggregate root.</param>
	/// <param name="budgetDbContext">Repository that manages Budget aggregate root.</param>
	public HandleCancellBudgetTransaction(IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository, BudgetDbContext budgetDbContext)
	{
		this.budgetTransactionRepository = budgetTransactionRepository;
		this.budgetDbContext = budgetDbContext;
	}

	/// <inheritdoc/>
	public async Task Handle(CancellBudgetTransaction command, CancellationToken cancellationToken)
	{
		var transactionId = new TransactionId(command.Id);
		var transaction = this.budgetDbContext.Transaction
			.FirstOrDefault(x => x.Id == transactionId);

		if (transaction != null)
		{
			transaction.CancellTransaction();
			await this.budgetTransactionRepository.Persist(transaction);
		}
	}
}