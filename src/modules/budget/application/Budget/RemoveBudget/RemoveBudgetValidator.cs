using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.RemoveBudget;

/// <summary>
/// Remove Budget validator class.
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
			.MustAsync((x, cancellation) => this.IsExistingBudget(x.Id, executionContextAccessor, cancellation))
			.WithMessage("{BudgetId} don't exists. Choose a different number id");
	}

	private async Task<bool> IsExistingBudget(Guid id, IExecutionContextAccessor executionContextAccessor, CancellationToken cancellation)
	{
		////return await this.budgetDbContext.Budget.AnyAsync(b => b.Id.Equals(new BudgetId(id)) && b.UserId.Equals(executionContextAccessor.GetUserId()), cancellation);
		//// TODO: Uncomment /\ and remove the line below \/ only when the create budget endpoint has automatic assignment of the creating user's ID to the UserId property.
		return await this.budgetDbContext.Budget.AnyAsync(b => b.Id.Equals(new BudgetId(id)), cancellation);
	}
}