using System.Net;
using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;

/// <summary>
/// GetBudgetsValidator class.
/// </summary>
public class GetBudgetDetailsValidator : AbstractValidator<GetBudgetDetails>
{
	private readonly IKeycloakService keycloakService;
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetDetailsValidator"/> class.
	/// </summary>
	/// <param name="keycloakService">Keycloak service to validate users ids.</param>
	/// <param name="budgetDbContext">Budget repository to validate budget ids.</param>
	public GetBudgetDetailsValidator(IKeycloakService keycloakService, BudgetDbContext budgetDbContext)
	{
		this.keycloakService = keycloakService;
		this.budgetDbContext = budgetDbContext;
		this.RuleFor(budget => budget.Id)
			.NotEmpty()
			.MustAsync(this.IsBudgetExists)
			.MustAsync(this.IsUserExists).WithMessage("userId does not exist");
	}

	private async Task<bool> IsUserExists(Guid id, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(id);
		var budgetOwnerId = this.budgetDbContext.Budget.Where(x => x.Id == budgetId).Select(x => x.UserId).FirstOrDefault();

		var response = await this.keycloakService.GetUserById(budgetOwnerId.ToString(), cancellationToken);

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

	private async Task<bool> IsBudgetExists(Guid budgetGuid, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(budgetGuid);
		var budget = await this.budgetDbContext.Budget
			.SingleOrDefaultAsync(x => x.Id == budgetId, cancellationToken: cancellationToken);

		return budget != null;
	}
}