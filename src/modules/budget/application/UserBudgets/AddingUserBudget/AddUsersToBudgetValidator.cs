using System.Net;
using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Application.GettingUsers;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;

/// <summary>
/// Budget Users validator class.
/// </summary>
public class AddUsersToBudgetValidator : AbstractValidator<AddUsersToBudget>
{
	private readonly IKeycloakService keycloakService;
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="AddUsersToBudgetValidator"/> class.
	/// </summary>
	/// <param name="keycloakService">Keycloak service to validate users ids.</param>
	/// <param name="budgetDbContext">Budget repository to validate budget ids.</param>
	public AddUsersToBudgetValidator(IKeycloakService keycloakService, BudgetDbContext budgetDbContext)
	{
		this.keycloakService = keycloakService;
		this.budgetDbContext = budgetDbContext;

		this.RuleFor(x => x.BudgetId)
			.NotEmpty()
			.NotNull()
			.MustAsync(this.IsBudgetExists)
			.WithMessage("{PropertyName}: Budget with id {PropertyValue} does not exist.");

		this.RuleFor(x => x.UsersIds)
			.NotEmpty()
			.NotNull()
			.CustomAsync(this.AreExistingUsersIds);
	}

	private async Task<bool> IsBudgetExists(Guid budgetGuid, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(budgetGuid);
		var budget = await this.budgetDbContext.Budget
			.SingleOrDefaultAsync(x => x.Id == budgetId, cancellationToken: cancellationToken);

		return budget != null;
	}

	private async Task<UserInfo?> IsUserExists(Guid id, CancellationToken cancellationToken)
	{
		var response = await this.keycloakService.GetUserById(id.ToString(), cancellationToken);

		if (response.StatusCode == HttpStatusCode.NotFound)
		{
			return null;
		}

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException(response.ToString());
		}

		string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

		var deserializedUsers = JsonConvert.DeserializeObject<UserInfo>(responseContent);

		return deserializedUsers;
	}

	private async Task<List<Guid>> GetAllBudgetUsers(Guid budgetGuid, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(budgetGuid);

		var budgetUsersIds = await this.budgetDbContext.UserBudget
			.Where(x => x.BudgetId == budgetId)
		.Select(x => x.UserId.Value)
		.ToListAsync(cancellationToken: cancellationToken);
		return budgetUsersIds;
	}

	private async Task AreExistingUsersIds(Guid[] usersIds, ValidationContext<AddUsersToBudget> validationContext, CancellationToken cancellationToken)
	{
		var budgetGuid = validationContext.InstanceToValidate.BudgetId;

		usersIds.GroupBy(x => x)
		.Where(x => x.Count() > 1)
		.Select(x => $"User id {x} duplicated.")
		.ToList()
		.ForEach(x => validationContext.AddFailure(x));

		var budgetUsersRoles = await this.GetAllBudgetUsers(budgetGuid, cancellationToken);

		foreach (Guid userId in usersIds)
		{
			var user = await this.IsUserExists(userId, cancellationToken);

			if (user == null)
			{
				validationContext.AddFailure(new FluentValidation.Results.ValidationFailure("UsersIds", $"User with id {userId} does not exist."));
				break;
			}

			if (budgetUsersRoles.Contains(userId))
			{
				validationContext.AddFailure(new FluentValidation.Results.ValidationFailure("UsersIds", $"User with id {userId} has already been added earlier."));
			}
		}
	}
}