using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;

/// <summary>
/// Budget validator class.
/// </summary>
public class CreateBudgetValidator : AbstractValidator<CreateBudget>
{
	private readonly IBudgetRepository budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="CreateBudgetValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages Budget aggregate root.</param>
	public CreateBudgetValidator(IBudgetRepository budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.RuleFor(budget => budget.Id).NotEmpty().NotNull();
		this.RuleFor(budget => budget.Name).NotEmpty().NotNull().WithMessage("{PropertyName} must not be empty");
		this.RuleFor(budget => new { budget.StartDate, budget.EndDate }).Must(x => x.StartDate < x.EndDate).WithMessage("The start date must be earlier than the end date");
		this.RuleFor(budget => budget.Name).Must(x => !this.budgetRepository.ExistsByName(x)).WithMessage("{PropertyName} already exists. Choose a different name");
	}
}