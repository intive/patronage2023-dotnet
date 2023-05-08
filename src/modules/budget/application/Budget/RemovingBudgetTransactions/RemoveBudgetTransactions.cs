using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.RemovingBudgetTransactions;

/// <summary>
/// Remove Budget Transactions command.
/// </summary>
/// <param name="Id">Budget identifier.</param>
public record RemoveBudgetTransactions(Guid Id) : ICommand;

/// <summary>
/// Remove Budget Transactions Command Handler.
/// </summary>
public class HandleCancellBudgetTransactions : ICommandHandler<RemoveBudgetTransactions>
{
	private readonly IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository;
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleCancellBudgetTransactions"/> class.
	/// </summary>
	/// <param name="budgetTransactionRepository">Repository that manages Budget Transaction aggregate root.</param>
	/// <param name="budgetDbContext">Repository that manages Budget aggregate root.</param>
	public HandleCancellBudgetTransactions(IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository, BudgetDbContext budgetDbContext)
	{
		this.budgetTransactionRepository = budgetTransactionRepository;
		this.budgetDbContext = budgetDbContext;
	}

	/// <inheritdoc/>
	public async Task Handle(RemoveBudgetTransactions command, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(command.Id);
		var transactions = await this.budgetDbContext.Transaction
			.Where(x => x.BudgetId == budgetId)
			.ToListAsync(cancellationToken: cancellationToken);

		foreach (var transaction in transactions)
		{
			transaction.SoftRemove();
			await this.budgetTransactionRepository.Persist(transaction);
		}
	}
}