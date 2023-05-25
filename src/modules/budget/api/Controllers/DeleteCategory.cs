using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Api.Controllers;

/// <summary>
/// Represents a command to delete a category from a specific budget.
/// </summary>
/// <param name="BudgetId">The ID of the budget from which to delete the category.</param>
/// <param name="CategoryName">The name of the category to delete.</param>
public record DeleteCategory(BudgetId BudgetId, string CategoryName) : ICommand;

/// <summary>
/// Delete transaction category from budget.
/// </summary>
public class HandleDeleteTransactionCategory : ICommandHandler<DeleteCategory>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleDeleteTransactionCategory"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages Budget aggregate root.</param>
	public HandleDeleteTransactionCategory(IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetRepository = budgetRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(DeleteCategory command, CancellationToken cancellationToken)
	{
		await Task.CompletedTask;
	}
}