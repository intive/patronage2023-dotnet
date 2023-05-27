using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.AddingTransactionCategory;

/// <summary>
/// Represents a command to add a category to a specific budget.
/// </summary>
/// <param name="BudgetId">The ID of the budget to which the category will be added.</param>
/// <param name="Icon">The transaction category icon.</param>
/// <param name="CategoryName">The transaction category name.</param>
public record AddCategory(BudgetId BudgetId, string Icon, string CategoryName) : ICommand;

/// <summary>
/// Create Budget.
/// </summary>
public class HandleAddTransactionCategoryToBudget : ICommandHandler<AddCategory>
{
	private readonly IRepository<TransactionCategoryAggregate, TransactionCategoryId> transactionCategoryRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleAddTransactionCategoryToBudget"/> class.
	/// </summary>
	/// <param name="transactionCategoryRepository">Repository that manages Budget aggregate root.</param>
	public HandleAddTransactionCategoryToBudget(IRepository<TransactionCategoryAggregate, TransactionCategoryId> transactionCategoryRepository)
	{
		this.transactionCategoryRepository = transactionCategoryRepository;
	}

	/// <summary>
	/// Handles the command asynchronously.
	/// </summary>
	/// <param name="command">The command to handle.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public async Task Handle(AddCategory command, CancellationToken cancellationToken)
	{
		var category = TransactionCategoryAggregate.Create(new TransactionCategoryId(Guid.NewGuid()), command.BudgetId, command.Icon, command.CategoryName);
		await this.transactionCategoryRepository.Persist(category);
	}
}