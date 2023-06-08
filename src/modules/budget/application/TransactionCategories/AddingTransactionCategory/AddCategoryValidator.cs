using System.Drawing;
using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.Provider;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.AddingTransactionCategory;

/// <summary>
/// Validator for the AddCategory command.
/// </summary>
public class AddCategoryValidator : AbstractValidator<AddTransactionCategory>
{
	private readonly ICategoryProvider categoryProvider;
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="AddCategoryValidator"/> class.
	/// </summary>
	/// <param name="categoryProvider">The provider used to access category information.</param>
	/// <param name="budgetRepository">Repository that manages BudgetAggregate.</param>
	public AddCategoryValidator(IRepository<BudgetAggregate, BudgetId> budgetRepository, ICategoryProvider categoryProvider)
	{
		this.budgetRepository = budgetRepository;
		this.categoryProvider = categoryProvider;
		this.RuleFor(command => command.BudgetId).NotEmpty().WithMessage("Budget ID is required.");
		this.RuleFor(command => command.BudgetId).MustAsync(this.BudgetExists).WithMessage("Budget doesn't exist.");
		this.RuleFor(command => command.Icon).NotEmpty().WithMessage("Icon is required.");
		this.RuleFor(command => command.Icon.IconName).NotEmpty().WithMessage("Icon name is required.");
		this.RuleFor(command => command.Icon.Foreground).NotEmpty().Must(this.ColorExists).WithMessage("Foreground should be hex code.");
		this.RuleFor(command => command.Icon.Background).NotEmpty().Must(this.ColorExists).WithMessage("Background should be hex code.");
		this.RuleFor(command => command.CategoryType).NotEmpty().WithMessage("Category name is required.");
		this.RuleFor(command => new { command.BudgetId, command.CategoryType })
			.Must((x) => !this.CategoryNameExists(x.BudgetId, x.CategoryType.CategoryName))
			.WithMessage("Category Name must be unique.");
	}

	private async Task<bool> BudgetExists(BudgetId budgetId, CancellationToken cancellationToken)
	{
		var budget = await this.budgetRepository.GetById(budgetId);
		return budget != null;
	}

	private bool CategoryNameExists(BudgetId budgetId, string categoryName)
	{
		return this.categoryProvider.GetForBudget(budgetId).Any(x => x.Name == categoryName);
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