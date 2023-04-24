using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;

/// <summary>
/// Budget validator class.
/// </summary>
public class CreateBudgetValidator : AbstractValidator<CreateBudget>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="CreateBudgetValidator"/> class.
	/// </summary>
	/// <param name="budgetDbContext">BudgetDbContext.</param>
	public CreateBudgetValidator(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
		this.RuleFor(budget => budget.Id).NotEmpty().NotNull();
		this.RuleFor(budget => budget.Name).NotEmpty().NotNull().WithMessage("{PropertyName} must not be empty");
		this.RuleFor(budget => new { budget.Period.StartDate, budget.Period.EndDate }).Must(x => x.StartDate < x.EndDate).WithMessage("The start date must be earlier than the end date");
		this.RuleFor(budget => budget.Name).Must(x => !this.budgetDbContext.Budget.Any(b => b.Name.Equals(x))).WithMessage("{PropertyName} already exists. Choose a different name");
	}
}