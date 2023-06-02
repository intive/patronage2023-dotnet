using FluentValidation;

using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Domain;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.DeletingTransactionCategory;

/// <summary>
/// 2.
/// </summary>
public class DeleteTransactionCategoryValidator : AbstractValidator<DeleteTransactionCategory>
{
	private readonly IRepository<TransactionCategoryAggregate, TransactionCategoryId> transactionCategoryRepository;
	private readonly BudgetDbContext dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="DeleteTransactionCategoryValidator"/> class.
	/// </summary>
	/// <param name="transactionCategoryRepository">Repository.</param>
	/// <param name="dbContext">The BudgetDbContext.</param>
	public DeleteTransactionCategoryValidator(IRepository<TransactionCategoryAggregate, TransactionCategoryId> transactionCategoryRepository, BudgetDbContext dbContext)
	{
		this.transactionCategoryRepository = transactionCategoryRepository;
		this.dbContext = dbContext;
		this.RuleFor(category => category.CategoryId)
			.NotEmpty().NotNull()
			.MustAsync(this.CategoryExsists).WithMessage("Category not belong to the budget.");
		this.RuleFor(category => category)
			.NotEmpty().NotNull()
			.MustAsync(this.TransactionExistsForCategory)
			.WithMessage("Cannot delete the category because it is used in one or more transactions.");
	}

	private async Task<bool> CategoryExsists(TransactionCategoryId categoryId, CancellationToken cancellationToken)
	{
		var category = await this.transactionCategoryRepository.GetById(categoryId);
		return category != null;
	}

	private async Task<bool> TransactionExistsForCategory(DeleteTransactionCategory categoryData, CancellationToken cancellationToken)
	{
		var category = await this.transactionCategoryRepository.GetById(categoryData.CategoryId);
		return await this.dbContext.Transaction
			.AnyAsync(transaction => transaction.BudgetId == categoryData.BudgetId && transaction.Name == category!.Name, cancellationToken: cancellationToken);
	}
}