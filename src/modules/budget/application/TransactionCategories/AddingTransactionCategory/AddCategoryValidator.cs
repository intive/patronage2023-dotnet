using System.Drawing;
using FluentValidation;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.AddingTransactionCategory;

/// <summary>
/// Validator for the AddCategory command.
/// </summary>
public class AddCategoryValidator : AbstractValidator<AddCategory>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AddCategoryValidator"/> class.
	/// </summary>
	public AddCategoryValidator()
	{
		this.RuleFor(command => command.BudgetId).NotNull().NotEmpty().WithMessage("Budget ID is required.");
		this.RuleFor(command => command.Icon).NotNull().NotEmpty().WithMessage("Icon is required.");
		this.RuleFor(command => command.Icon.IconName).NotNull().NotEmpty().WithMessage("Icon name is required.");
		this.RuleFor(command => command.Icon.Foreground).NotNull().NotEmpty().Must(this.ColorExists).WithMessage("Foreground should be hex code.");
		this.RuleFor(command => command.Icon.Background).NotNull().NotEmpty().Must(this.ColorExists).WithMessage("Background should be hex code.");
		this.RuleFor(command => command.CategoryName).NotNull().NotEmpty().WithMessage("Category name is required.");
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