using System.Drawing;
using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Abstractions.Queries;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.AddingTransactionCategory;

/// <summary>
/// Validator for the AddCategory command.
/// </summary>
public class AddCategoryValidator : AbstractValidator<AddTransactionCategory>
{
	private readonly IQueryBus queryBus;
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="AddCategoryValidator"/> class.
	/// </summary>
	/// <param name="queryBus">The QueryBus.</param>
	/// <param name="budgetRepository">Repository that manages BudgetAggregate.</param>
	public AddCategoryValidator(IQueryBus queryBus, IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.queryBus = queryBus;
		this.budgetRepository = budgetRepository;
		this.RuleFor(command => command.BudgetId).NotNull().NotEmpty().WithMessage("Budget ID is required.");
		this.RuleFor(budget => budget.BudgetId).MustAsync(this.BudgetExists).WithMessage("Budget doesn't exist.");
		this.RuleFor(command => command.Icon).NotNull().NotEmpty().WithMessage("Icon is required.");
		this.RuleFor(command => command.Icon.IconName).NotNull().NotEmpty().WithMessage("Icon name is required.");
		this.RuleFor(command => command.Icon.Foreground).NotNull().NotEmpty().Must(this.ColorExists).WithMessage("Foreground should be hex code.");
		this.RuleFor(command => command.Icon.Background).NotNull().NotEmpty().Must(this.ColorExists).WithMessage("Background should be hex code.");
		this.RuleFor(command => command.CategoryType).NotNull().NotEmpty().WithMessage("Category name is required.");
		this.RuleFor(command => new { command.BudgetId, command.CategoryType })
			.MustAsync(async (x, cancellation) => !await this.CategoryNameExists(x.BudgetId, x.CategoryType.CategoryName, cancellation))
			.WithMessage("Category Name must be unique.");
	}

	private async Task<bool> BudgetExists(BudgetId budgetId, CancellationToken cancellationToken)
	{
		var budget = await this.budgetRepository.GetById(budgetId);
		return budget != null;
	}

	private async Task<bool> CategoryNameExists(BudgetId budgetId, string categoryName, CancellationToken cancellationToken)
	{
		var query = new GetTransactionCategories(budgetId);
		var categories = await this.queryBus.Query<GetTransactionCategories, TransactionCategoriesInfo>(query);
		return categories.Categories!.Any(x => x.Name == categoryName);
	}

	private bool ColorExists(string hexColor)
	{
		try
		{
			Color color = ColorTranslator.FromHtml(hexColor);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}