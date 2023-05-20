using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.UpdateUserBudgetFavourite;

/// <summary>
/// Bool validator.
/// </summary>
public class UpdateUserBudgetFavouriteValidator : AbstractValidator<UpdateUserBudgetFavourite>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="UpdateUserBudgetFavouriteValidator"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget repository to validate budget ids.</param>
	public UpdateUserBudgetFavouriteValidator(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;

		this.RuleFor(x => x.BudgetId)
			.NotEmpty()
			.NotNull()
			.MustAsync(this.IsBudgetExists)
			.WithMessage("{PropertyName}: Budget with id {PropertyValue} does not exist.");

		this.RuleFor(x => x.IsFavourite)
			.NotNull()
			.NotEmpty();
	}

	private async Task<bool> IsBudgetExists(Guid budgetGuid, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(budgetGuid);
		var budget = await this.budgetDbContext.Budget
			.SingleOrDefaultAsync(x => x.Id == budgetId, cancellationToken: cancellationToken);

		return budget != null;
	}
}