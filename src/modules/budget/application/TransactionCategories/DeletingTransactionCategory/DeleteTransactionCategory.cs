using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.DeletingTransactionCategory;

/// <summary>
/// Represents a command to delete a category from a specific budget.
/// </summary>
/// <param name="CategoryId">The id of the category to delete.</param>
/// <param name="BudgetId">The id of the budget which category will be deleted for.</param>
public record DeleteTransactionCategory(TransactionCategoryId CategoryId, BudgetId BudgetId) : ICommand;

/// <summary>
/// Handles the command for deleting a transaction category.
/// </summary>
public class HandleDeleteTransactionCategory : ICommandHandler<DeleteTransactionCategory>
{
	private readonly IRepository<TransactionCategoryAggregate, TransactionCategoryId> transactionCategoryRepository;
	private readonly BudgetDbContext dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleDeleteTransactionCategory"/> class.
	/// </summary>
	/// <param name="transactionCategoryRepository">Repository that manages Budget aggregate root.</param>
	/// <param name="dbContext">The BudgetDbContext.</param>
	public HandleDeleteTransactionCategory(IRepository<TransactionCategoryAggregate, TransactionCategoryId> transactionCategoryRepository, BudgetDbContext dbContext)
	{
		this.transactionCategoryRepository = transactionCategoryRepository;
		this.dbContext = dbContext;
	}

	/// <inheritdoc/>
	public async Task Handle(DeleteTransactionCategory command, CancellationToken cancellationToken)
	{
		var category = await this.transactionCategoryRepository.GetById(command.CategoryId);
		category!.DeleteCategory();
		await this.transactionCategoryRepository.Remove(category);
	}
}