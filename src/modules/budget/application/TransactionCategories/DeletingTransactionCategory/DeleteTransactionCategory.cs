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
public record DeleteTransactionCategory(TransactionCategoryId CategoryId) : ICommand;

/// <summary>
/// Delete transaction category from budget.
/// </summary>
public class HandleDeleteTransactionCategory : ICommandHandler<DeleteTransactionCategory>
{
	private readonly IRepository<TransactionCategoryAggregate, TransactionCategoryId> transactionCategoryRepository;
	private readonly BudgetDbContext dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleDeleteTransactionCategory"/> class.
	/// </summary>
	/// <param name="transactionCategoryRepository">Repository that manages Budget aggregate root.</param>
	/// <param name="dbContext">DbContext.</param>
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
		this.dbContext.BudgetTransactionCategory.Remove(category);
		await this.dbContext.SaveChangesAsync(cancellationToken);
	}
}