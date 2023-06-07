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
/// AddUserBudgetList validator.
/// </summary>
public class AddUserBudgetListValidator : AbstractValidator<AddUserBudgetList>
{
	private readonly IKeycloakService keycloakService;
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="AddUserBudgetListValidator"/> class.
	/// </summary>
	/// <param name="keycloakService">Keycloak service to validate users ids.</param>
	/// <param name="budgetDbContext">Budget repository to validate budget ids.</param>
	public AddUserBudgetListValidator(IKeycloakService keycloakService, BudgetDbContext budgetDbContext)
	{
		this.keycloakService = keycloakService;
		this.budgetDbContext = budgetDbContext;

		this.RuleFor(x => x.BudgetId)
			.NotEmpty()
			.NotNull()
			.MustAsync(this.IsBudgetExists)
			.WithMessage("{PropertyName}: Budget with id {PropertyValue} does not exist.").WithErrorCode("1.11");

		this.RuleFor(x => x.UsersIds)
			.NotNull()
			.Custom(this.IsUserIdDupliacted)
			.CustomAsync(this.AreExistingUsersIds)
			.CustomAsync(this.IsOwnerInList);
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

	private void IsUserIdDupliacted(Guid[] usersIds, ValidationContext<AddUserBudgetList> validationContext)
	{
		var budgetGuid = validationContext.InstanceToValidate.BudgetId;

		usersIds.GroupBy(x => x)
		.Where(x => x.Count() > 1)
		.Select(x => $"User id {x.Key} duplicated.")
		.ToList()
		.ForEach(validationContext.AddFailure);
	}

	private async Task AreExistingUsersIds(Guid[] usersIds, ValidationContext<AddUserBudgetList> validationContext, CancellationToken cancellationToken)
	{
		foreach (Guid userId in usersIds)
		{
			var user = await this.IsUserExists(userId, cancellationToken);

			if (user == null)
			{
				validationContext.AddFailure(new FluentValidation.Results.ValidationFailure("UsersIds", $"User with id {userId} does not exist."));
				break;
			}
		}
	}

	private async Task IsOwnerInList(Guid[] usersIds, ValidationContext<AddUserBudgetList> validationContext, CancellationToken cancellationToken)
	{
		var budgetGuid = validationContext.InstanceToValidate.BudgetId;
		var budgetId = new BudgetId(budgetGuid);

		var budgetOwnerId = await this.budgetDbContext.Budget
			.Where(x => x.Id.Equals(budgetId))
			.Select(x => x.UserId.Value)
			.SingleOrDefaultAsync(cancellationToken: cancellationToken);

		bool isOwnerInList = Array.Exists(usersIds, userId => userId.Equals(budgetOwnerId));

		if (isOwnerInList)
		{
			validationContext.AddFailure(new FluentValidation.Results.ValidationFailure("UsersIds", $"Budget owner with id {budgetOwnerId} can not be added to list."));
		}
	}
}