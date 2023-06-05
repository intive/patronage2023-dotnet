using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

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
	/// <param name="executionContextAccessor">Context to get user id from token.</param>
	public CreateBudgetValidator(BudgetDbContext budgetDbContext, IExecutionContextAccessor executionContextAccessor)
	{
		this.budgetDbContext = budgetDbContext;

		this.RuleFor(budget => budget.Id)
			.NotEmpty()
			.NotNull();

		this.RuleFor(budget => budget.Name)
			.NotEmpty().WithErrorCode("1.2")
			.NotNull()
			.Length(3, 30).WithErrorCode("1.3");

		this.RuleFor(budget => new { budget.Name, budget.UserId })
			.MustAsync((x, cancellation) => this.NoExistingBudget(x.Name, executionContextAccessor, cancellation))
			.WithMessage("{PropertyName} already exists. Choose a different name").WithErrorCode("1.4");

		this.RuleFor(budget => budget.Period.StartDate)
			.NotEmpty().WithErrorCode("1.5");

		this.RuleFor(budget => budget.Period.EndDate)
			.NotEmpty().WithErrorCode("1.6");

		this.RuleFor(budget => new { budget.Period.StartDate, budget.Period.EndDate })
			.Must(x => x.StartDate <= x.EndDate)
			.WithMessage("The start date must be earlier than the end date").WithErrorCode("1.7");

		this.RuleFor(budget => budget.Limit)
			.NotEmpty().WithErrorCode("1.8")
			.NotNull();

		this.RuleFor(budget => budget.Limit.Value)
			.GreaterThan(0).WithErrorCode("1.9");

		this.RuleFor(budget => budget.Limit.Currency)
			.IsInEnum().WithMessage("The selected currency is not supported.").WithErrorCode("1.10");
	}

	private async Task<bool> NoExistingBudget(string name, IExecutionContextAccessor executionContextAccessor, CancellationToken cancellation)
	{
		bool anyExistingBudget = await this.budgetDbContext.Budget.AnyAsync(b => b.Name.Equals(name) && b.UserId.Equals(executionContextAccessor.GetUserId()), cancellation);
		return !anyExistingBudget;
	}
}