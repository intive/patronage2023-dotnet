using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgetTransactions;

/// <summary>
/// BudgetTransactionAggregateList class represents a command that holds a list of BudgetTransactionAggregate.
/// </summary>
/// <param name="ListOfBudgetTransactions">A list of BudgetTransactionAggregate objects.</param>
public record BudgetTransactionAggregateList(List<BudgetTransactionAggregate> ListOfBudgetTransactions) : ICommand;

/// <summary>
/// The HandleBudgetAggregateList class handles BudgetAggregateList command.
/// It implements the ICommandHandler interface to handle BudgetAggregateList commands.
/// </summary>
public class HandleBudgetTransactionAggregateList : ICommandHandler<BudgetTransactionAggregateList>
{
	private readonly IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleBudgetTransactionAggregateList"/> class.
	/// </summary>
	/// <param name="budgetTransactionRepository">Repository that manages Budget transaction aggregate root.</param>
	public HandleBudgetTransactionAggregateList(IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository)
	{
		this.budgetTransactionRepository = budgetTransactionRepository;
	}

	/// <summary>
	/// Handles the provided BudgetTransactionAggregateList command.
	/// This involves persisting the list of BudgetTransactionAggregate objects contained in the command to the repository.
	/// </summary>
	/// <param name="command">The BudgetTransactionAggregateList command to be handled.</param>
	/// <param name="cancellationToken">A token that may be used to cancel the handle operation.</param>
	/// <returns>A <see cref="Task"/>Representing the asynchronous operation of handling the command.</returns>
	public async Task Handle(BudgetTransactionAggregateList command, CancellationToken cancellationToken)
	{
		foreach (var transaction in command.ListOfBudgetTransactions)
		{
			await this.budgetTransactionRepository.Persist(transaction);
		}
	}
}