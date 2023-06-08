using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.DeletingTransactionCategory;

/// <summary>
/// Validator for the DeleteTransactionCategory command.
/// </summary>
public class DeleteTransactionCategoryValidator : AbstractValidator<DeleteTransactionCategory>
{
	private readonly IRepository<TransactionCategoryAggregate, TransactionCategoryId> transactionCategoryRepository;
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;
	private readonly BudgetDbContext dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="DeleteTransactionCategoryValidator"/> class.
	/// </summary>
	/// <param name="transactionCategoryRepository">Repository that manages TransactionCategoryAggregate.</param>
	/// <param name="dbContext">The BudgetDbContext.</param>
	/// <param name="budgetRepository">Repository that manages BudgetAggregate.</param>
	public DeleteTransactionCategoryValidator(IRepository<TransactionCategoryAggregate, TransactionCategoryId> transactionCategoryRepository, BudgetDbContext dbContext, IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.transactionCategoryRepository = transactionCategoryRepository;
		this.dbContext = dbContext;
		this.budgetRepository = budgetRepository;

		this.RuleFor(budget => budget.BudgetId).NotEmpty().MustAsync(this.BudgetExists).WithMessage("Budget doesn't exist.");
		this.RuleFor(category => category.CategoryId)
			.NotEmpty()
			.DependentRules(() =>
			{
				this.RuleFor(category => category.CategoryId)
					.MustAsync(this.CategoryExists)
					.DependentRules(() =>
					{
						this.RuleFor(category => category)
							.MustAsync(this.TransactionExistsForCategory)
							.WithMessage("Cannot delete the category because it is used in one or more transactions.");
					})
					.WithMessage("Category does not belong to the budget.");
			})
			.WithMessage("Built-in category cannot be deleted.");
	}

	private async Task<bool> BudgetExists(BudgetId budgetId, CancellationToken cancellationToken)
	{
		var budget = await this.budgetRepository.GetById(budgetId);
		return budget != null;
	}

	private async Task<bool> CategoryExists(TransactionCategoryId categoryId, CancellationToken cancellationToken)
	{
		var category = await this.transactionCategoryRepository.GetById(categoryId);
		return category != null;
	}

	private async Task<bool> TransactionExistsForCategory(DeleteTransactionCategory categoryData, CancellationToken cancellationToken)
	{
		var category = await this.transactionCategoryRepository.GetById(categoryData.CategoryId);

		return !await this.dbContext.Transaction
			.AnyAsync(transaction => transaction.BudgetId == categoryData.BudgetId && transaction.CategoryType == category!.CategoryType, cancellationToken);
	}
}