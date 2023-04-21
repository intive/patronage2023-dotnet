using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;

/// <summary>
/// Budget validator class.
/// </summary>
public class CreateBudgetValidator : AbstractValidator<CreateBudget>
{
	private readonly IBudgetRepository budgetRepository;
	private readonly IExecutionContextAccessor executionContextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="CreateBudgetValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages Budget aggregate root.</param>
	/// <param name="executionContextAccessor">Implementation of IExecutionContextAccessor which uses JWT token to obtain user Id.</param>
	public CreateBudgetValidator(IBudgetRepository budgetRepository, IExecutionContextAccessor executionContextAccessor)
	{
		this.budgetRepository = budgetRepository;
		this.executionContextAccessor = executionContextAccessor;

		this.RuleFor(budget => budget.Id)
			.NotEmpty()
			.NotNull();
		this.RuleFor(budget => budget.Name)
			.NotEmpty()
			.NotNull()
			.Length(3, 30);
		this.RuleFor(budget => budget.Name)
			.Must(x => !this.budgetRepository.ExistsByName(executionContextAccessor.GetUserId(), x))
			.WithMessage("{PropertyName} already exists. Choose a different name");
		this.RuleFor(budget => budget.Period)
			.NotEmpty()
			.NotNull();
		this.RuleFor(budget => new { budget.Period.StartDate, budget.Period.EndDate })
			.Must(x => x.StartDate <= x.EndDate)
			.WithMessage("The start date must be earlier than the end date");
		this.RuleFor(budget => budget.Limit)
			.NotEmpty()
			.NotNull();
		this.RuleFor(budget => budget.Limit.Value)
			.GreaterThan(0);
		this.RuleFor(budget => budget.Limit.Currency)
			.IsInEnum();
		this.RuleFor(budget => budget.Description)
			.MaximumLength(50);
		this.RuleFor(budget => budget.IconName.Length)
			.LessThan(256);
	}
}