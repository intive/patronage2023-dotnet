using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;

/// <summary>
/// BudgetAggregateList class represents a command that holds a list of BudgetAggregate.
/// </summary>
/// <param name="ListOfBudgets">A list of BudgetAggregate objects.</param>
public record BudgetAggregateList(List<BudgetAggregate> ListOfBudgets) : ICommand;

/// <summary>
/// The HandleBudgetAggregateList class handles BudgetAggregateList command.
/// It implements the ICommandHandler interface to handle BudgetAggregateList commands.
/// </summary>
public class HandleBudgetAggregateList : ICommandHandler<BudgetAggregateList>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleBudgetAggregateList"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages Budget aggregate root.</param>
	public HandleBudgetAggregateList(IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetRepository = budgetRepository;
	}

	/// <summary>
	/// Handles the provided BudgetAggregateList command.
	/// This involves persisting the list of BudgetAggregate objects contained in the command to the repository.
	/// </summary>
	/// <param name="command">The BudgetAggregateList command to be handled.</param>
	/// <param name="cancellationToken">A token that may be used to cancel the handle operation.</param>
	/// <returns>A <see cref="Task"/>Representing the asynchronous operation of handling the command.</returns>
	public async Task Handle(BudgetAggregateList command, CancellationToken cancellationToken)
	{
		foreach (var budget in command.ListOfBudgets)
		{
			await this.budgetRepository.Persist(budget);
		}
	}
}