using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.EditingBudget;

/// <summary>
/// Budget editing validator class.
/// </summary>
public class EditBudgetValidator : AbstractValidator<EditBudget>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="EditBudgetValidator"/> class.
	/// </summary>
	/// <param name="budgetDbContext">BudgetDbContext.</param>
	public EditBudgetValidator(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
		this.RuleFor(budget => budget.Id).MustAsync(this.IsBudgetExists).NotEmpty().NotNull();
		this.RuleFor(budget => new { budget.Id, budget.Name }).MustAsync(async (x, cancellation) => await this.IsNameUniqueWithinUserBudgets(x.Id, x.Name, cancellation)).WithMessage("Name already exists in your budgets. Choose a different name.");
		this.RuleFor(budget => new { budget.Period.StartDate, budget.Period.EndDate }).Must(x => x.StartDate <= x.EndDate).WithMessage("The start date must be earlier than the end date");
	}

	/// <summary>
	/// Checks if the name is unique.
	/// </summary>
	/// <returns>Result.</returns>
	private async Task<bool> IsNameUniqueWithinUserBudgets(BudgetId budgetId, string name, CancellationToken cancellationToken)
	{
		var budget = await this.budgetDbContext.Budget.FindAsync(budgetId);
		var budgets = this.budgetDbContext.Budget.Where(b => b.UserId == budget!.UserId);
		return !budgets.Any(x => x.Name == name && x.Id != budgetId);
	}

	/// <summary>
	/// Checks if budget with given id exists.
	/// </summary>
	/// <returns>Result.</returns>
	private async Task<bool> IsBudgetExists(BudgetId budgetId, CancellationToken cancellationToken)
	{
		return await this.budgetDbContext.Budget.FindAsync(budgetId) != null;
	}
}