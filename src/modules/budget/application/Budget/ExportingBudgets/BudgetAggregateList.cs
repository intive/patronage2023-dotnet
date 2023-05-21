using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

/// <summary>
/// Class BudgetAggregateList.
/// </summary>
/// <param name="ListOfBudgets">BudgetAggregate.</param>
public record BudgetAggregateList(List<BudgetAggregate> ListOfBudgets) : ICommand;

/// <summary>
/// Create Budget.
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

	/// <inheritdoc/>
	public async Task Handle(BudgetAggregateList command, CancellationToken cancellationToken)
	{
		await this.budgetRepository.Persist(command.ListOfBudgets);
	}
}