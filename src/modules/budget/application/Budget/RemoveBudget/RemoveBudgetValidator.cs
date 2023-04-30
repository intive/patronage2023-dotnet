using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.RemoveBudget;

/// <summary>
/// Budget validator class.
/// </summary>
public class RemoveBudgetValidator : AbstractValidator<RemoveBudget>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="RemoveBudgetValidator"/> class.
	/// </summary>
	/// <param name="budgetDbContext">BudgetDbContext.</param>
	/// <param name="executionContextAccessor">Context to get user id from token.</param>
	public RemoveBudgetValidator(BudgetDbContext budgetDbContext, IExecutionContextAccessor executionContextAccessor)
	{
		this.budgetDbContext = budgetDbContext;

		this.RuleFor(budget => budget.Id)
			.NotEmpty()
			.NotNull();

		this.RuleFor(budget => new { budget.Id })
			.MustAsync((x, cancellation) => this.NoExistingBudget(x.Id, executionContextAccessor, cancellation))
			.WithMessage("{BudgetId} don't exists. Choose a different number id");
	}

	private async Task<bool> NoExistingBudget(Guid id, IExecutionContextAccessor executionContextAccessor, CancellationToken cancellation)
	{
		bool anyExistingBudget = await this.budgetDbContext.Budget.AnyAsync(b => b.Id.Equals(id) && b.UserId.Equals(executionContextAccessor.GetUserId()), cancellation);
		return !anyExistingBudget;
	}
}