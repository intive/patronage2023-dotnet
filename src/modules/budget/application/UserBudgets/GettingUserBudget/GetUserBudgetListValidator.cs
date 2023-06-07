using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.GettingUserBudget;

/// <summary>
/// GetUserBudgetList validator.
/// </summary>
public class GetUserBudgetListValidator : AbstractValidator<GetUserBudgetList>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetUserBudgetListValidator"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget repository to validate budget ids.</param>
	public GetUserBudgetListValidator(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;

		this.RuleFor(x => x.BudgetId)
			.NotEmpty()
			.NotNull()
			.MustAsync(this.IsBudgetExists)
			.WithMessage("{PropertyName}: Budget with id {PropertyValue} does not exist.");
	}

	private async Task<bool> IsBudgetExists(BudgetId budgetId, CancellationToken cancellationToken)
	{
		var budget = await this.budgetDbContext.Budget
			.SingleOrDefaultAsync(x => x.Id.Equals(budgetId), cancellationToken: cancellationToken);

		return budget != null;
	}
}