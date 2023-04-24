using FluentValidation;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;

/// <summary>
/// Budget validator class.
/// </summary>
public class CreateBudgetValidator : AbstractValidator<CreateBudget>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CreateBudgetValidator"/> class.
	/// </summary>
	public CreateBudgetValidator()
	{
		this.RuleFor(budget => budget.Id)
		.NotEmpty()
		.NotNull();
		this.RuleFor(budget => budget.Name)
		.NotEmpty()
		.NotNull()
		.Length(3, 30);
		this.RuleFor(budget => budget.Period.StartDate)
		.NotEmpty();
		this.RuleFor(budget => budget.Period.EndDate)
		.NotEmpty();
		this.RuleFor(budget => new { budget.Period.StartDate, budget.Period.EndDate })
		.Must(x => x.StartDate <= x.EndDate)
		.WithMessage("The start date must be earlier than the end date");
		this.RuleFor(budget => budget.Limit)
		.NotEmpty()
		.NotNull();
		this.RuleFor(budget => budget.Limit.Value)
		.GreaterThan(0);
		this.RuleFor(budget => budget.Limit.Currency)
		.NotEmpty()
		.IsInEnum();
	}
}