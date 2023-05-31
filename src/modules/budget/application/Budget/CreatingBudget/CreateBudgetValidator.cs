using System.Net;
using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;

/// <summary>
/// Budget validator class.
/// </summary>
public class CreateBudgetValidator : AbstractValidator<CreateBudget>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IKeycloakService keycloakService;

	/// <summary>
	/// Initializes a new instance of the <see cref="CreateBudgetValidator"/> class.
	/// </summary>
	/// <param name="budgetDbContext">BudgetDbContext.</param>
	/// <param name="executionContextAccessor">Context to get user id from token.</param>
	/// <param name="keycloakService">Keycloak service to validate users ids.</param>
	public CreateBudgetValidator(BudgetDbContext budgetDbContext, IExecutionContextAccessor executionContextAccessor, IKeycloakService keycloakService)
	{
		this.budgetDbContext = budgetDbContext;
		this.keycloakService = keycloakService;

		this.RuleFor(budget => budget.Id)
			.NotEmpty()
			.NotNull();

		this.RuleFor(budget => budget.UserId)
			.NotEmpty()
			.NotNull()
			.MustAsync(this.IsUserExists).WithMessage("does not exists.");

		this.RuleFor(budget => budget.Name)
			.NotEmpty()
			.NotNull()
			.Length(3, 30);

		this.RuleFor(budget => new { budget.Name, budget.UserId })
			.MustAsync((x, cancellation) => this.NoExistingBudget(x.Name, executionContextAccessor, cancellation))
			.WithMessage("{PropertyName} already exists. Choose a different name");

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
			.IsInEnum().WithMessage("The selected currency is not supported.");
	}

	private async Task<bool> NoExistingBudget(string name, IExecutionContextAccessor executionContextAccessor, CancellationToken cancellation)
	{
		bool anyExistingBudget = await this.budgetDbContext.Budget.AnyAsync(b => b.Name.Equals(name) && b.UserId.Equals(executionContextAccessor.GetUserId()), cancellation);
		return !anyExistingBudget;
	}

	private async Task<bool> IsUserExists(Guid id, CancellationToken cancellationToken)
	{
		var response = await this.keycloakService.GetUserById(id.ToString(), cancellationToken);

		if (response.StatusCode == HttpStatusCode.NotFound)
		{
			return false;
		}

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException();
		}

		return true;
	}
}