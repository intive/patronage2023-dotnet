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
	private readonly IExecutionContextAccessor executionContextAccessor;
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
		this.executionContextAccessor = executionContextAccessor;

		this.RuleFor(budget => budget.Id)
			.NotEmpty()
			.NotNull();

		this.RuleFor(budget => budget.UserId)
			.NotEmpty()
			.NotNull()
			.MustAsync(this.IsUserExists).WithMessage("does not exists.");

		this.RuleFor(budget => budget.Name)
			.NotEmpty().WithErrorCode("1.2")
			.NotNull()
			.Length(3, 30).WithErrorCode("1.3");

		this.RuleFor(budget => new { budget.Name, budget.UserId })
			.MustAsync((x, cancellation) => this.NoExistingBudget(x.Name, executionContextAccessor, cancellation))
			.WithMessage("Name already exists. Choose a different name").WithErrorCode("1.4");

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
		var budgets = await this.budgetDbContext.Budget.Where(x => x.Name.Equals(name)).ToListAsync();
		return !budgets.Any(x => x.UserId.Value.Equals(executionContextAccessor.GetUserId()));
	}

	private async Task<bool> IsUserExists(Guid id, CancellationToken cancellationToken)
	{
		HttpResponseMessage response;
		try
		{
			response = await this.keycloakService.GetUserById(id.ToString(), cancellationToken);
		}
		catch (Exception)
		{
			throw new AppException("Something went wrong with keycloack");
		}

		if (response.StatusCode == HttpStatusCode.NotFound)
		{
			return false;
		}

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException(response.ToString());
		}

		return true;
	}
}