using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.EditingBudget;

/// <summary>
/// Budget validator class.
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
		this.RuleFor(budget => budget.Id).NotEmpty().NotNull().Must(x => this.budgetDbContext.Budget.Any(b => b.Id.Equals(x)));
		this.RuleFor(budget => new { budget.Period.StartDate, budget.Period.EndDate }).Must(x => x.StartDate < x.EndDate).WithMessage("The start date must be earlier than the end date");
		this.RuleFor(budget => budget.Name).Must(x => !this.budgetDbContext.Budget.Any(b => b.Name.Equals(x))).WithMessage("{PropertyName} already exists. Choose a different name");
		this.RuleFor(budget => new { budget.UserId, budget.Id }).Must(x => this.IsTheUserOwnerOfTheBudget(x.UserId, x.Id));
		this.RuleFor(budget => new { budget.UserId, budget.Id, budget.Name }).Must(x => this.IsNameUnique(x.Id, x.Name, x.UserId));
	}

	/// <summary>
	/// Checks if the name is unique.
	/// </summary>
	/// <returns>Result.</returns>
	private bool IsNameUnique(Guid budgetId, string name, Guid userId)
	{
		var budgets = this.budgetDbContext.Budget.Where(b => b.UserId == userId);
		var budget = budgets.FirstOrDefault(b => b.Name == name);
		if (budget == null || budget.Id == budgetId)
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// Checking if the user is an owner of the budget.
	/// </summary>
	/// <param name="userId">User Id.</param>
	/// <param name="budgetId">Budget Id.</param>
	/// <returns>Status.</returns>
	private bool IsTheUserOwnerOfTheBudget(Guid userId, Guid budgetId)
	{
		var budget = this.budgetDbContext.Find<BudgetAggregate>(userId);
		if (budget == null)
		{
			return false;
		}

		if (budget.UserId == userId)
		{
			return true;
		}

		return false;
	}
}