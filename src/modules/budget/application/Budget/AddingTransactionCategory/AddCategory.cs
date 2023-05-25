using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.AddingTransactionCategory;

/// <summary>
/// Represents a command to add a category to a specific budget.
/// </summary>
/// <param name="BudgetId">The ID of the budget to which the category will be added.</param>
public record AddCategory(BudgetId BudgetId) : ICommand;

/// <summary>
/// Create Budget.
/// </summary>
public class HandleAddTransactionCategoryToBudget : ICommandHandler<AddCategory>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleAddTransactionCategoryToBudget"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages Budget aggregate root.</param>
	public HandleAddTransactionCategoryToBudget(IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetRepository = budgetRepository;
	}

	/// <summary>
	/// Handles the command asynchronously.
	/// </summary>
	/// <param name="command">The command to handle.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public async Task Handle(AddCategory command, CancellationToken cancellationToken)
	{
		await Task.CompletedTask;
	}
}