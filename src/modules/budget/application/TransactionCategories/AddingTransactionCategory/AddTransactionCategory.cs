using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
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
/// <param name="CategoryType">The transaction category name.</param>
/// <param name="TransactionCategoryId">The transaction category id.</param>
public record AddTransactionCategory(TransactionCategoryId TransactionCategoryId, BudgetId BudgetId, Icon Icon, CategoryType CategoryType) : ICommand;

/// <summary>
/// Handles the command for adding a transaction category to a budget.
/// </summary>
public class HandleAddTransactionCategoryToBudget : ICommandHandler<AddTransactionCategory>
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
	public async Task Handle(AddTransactionCategory command, CancellationToken cancellationToken)
	{
		var category = TransactionCategoryAggregate.Create(command.TransactionCategoryId, command.BudgetId, command.Icon, command.CategoryType);
		await this.transactionCategoryRepository.Persist(category);
	}
}