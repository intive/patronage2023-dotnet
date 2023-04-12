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
		this.RuleFor(budget => budget.Id).NotEmpty().NotNull();
		this.RuleFor(budget => budget.Name).NotEmpty().NotNull();
	}
}