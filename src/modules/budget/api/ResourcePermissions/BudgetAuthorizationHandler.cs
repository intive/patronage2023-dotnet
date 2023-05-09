using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.GettingUserBudget;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Intive.Patronage2023.Modules.Budget.Api.ResourcePermissions;

/// <summary>
/// The BudgetAuthorizationHandler class handles authorization logic for
/// budget operations including create, read, and update.
/// It grants access to all operations for admins, checks user permission for a specific budget,
/// and grants access to the update operation only if the user is the budget owner.
/// </summary>
public class BudgetAuthorizationHandler :
	AuthorizationHandler<OperationAuthorizationRequirement, BudgetId>
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly IQueryBus queryBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetAuthorizationHandler"/> class.
	/// BudgetAuthorizationHandler.
	/// </summary>
	/// <param name="contextAccessor">.</param>
	/// <param name="budgetDbContext">Instance used to access the database.</param>
	/// <param name="queryBus">dfasdfasdf.</param>
	public BudgetAuthorizationHandler(IExecutionContextAccessor contextAccessor, BudgetDbContext budgetDbContext, IQueryBus queryBus)
	{
		this.contextAccessor = contextAccessor;
		this.queryBus = queryBus;
	}

	private BudgetId BudgetId { get; set; }

	private UserBudgetRoleInfo? Role { get; set; }

	/// <summary>
	/// This method handles the authorization logic for different operations on a budget,
	/// such as creating, reading, and updating. It first checks if the user is an admin,
	/// in which case it grants access to all operations.
	/// Then, it checks if the user has the necessary permission for the specific budget in question.
	/// Finally, it checks if the user role is budget owner before granting access to the update operation.
	/// </summary>
	/// <param name="context">The AuthorizationHandlerContext instance
	/// that contains information about the current authorization request.</param>
	/// <param name="requirement">The OperationAuthorizationRequirement instance
	/// that represents the requirement to be checked.</param>
	/// <param name="budgetId">The BudgetId instance that represents the ID of the budget
	/// on which the operation is to be performed.</param>
	/// <returns>The method returns a Task representing the asynchronous authorization operation.</returns>
	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		OperationAuthorizationRequirement requirement,
		BudgetId budgetId)
	{
		if (this.contextAccessor.IsUserAdmin())
		{
			context.Succeed(requirement);
			return;
		}

		this.BudgetId = budgetId;
		var query = new GetUserBudgetRole
		{
			BudgetId = this.BudgetId,
		};

		var userRole = await this.queryBus.Query<GetUserBudgetRole, UserBudgetRoleInfo?>(query);

		if (!this.IsUserContributor(userRole))
		{
			context.Fail();
			return;
		}

		switch (requirement.Name)
		{
			case nameof(Operations.Create):
				context.Succeed(requirement);
				break;

			case nameof(Operations.Read):
				context.Succeed(requirement);
				break;

			case nameof(Operations.Update):
				if (!this.IsUserBudgetOwner(userRole))
				{
					context.Fail();
					break;
				}

				context.Succeed(requirement);
				break;

			default:
				throw new ArgumentException("Unknown permission requirement.", nameof(requirement));
		}
	}

	private bool IsUserContributor(UserBudgetRoleInfo? userBudgetRoleInfo) =>
		userBudgetRoleInfo?.UserRole != null;

	private bool IsUserBudgetOwner(UserBudgetRoleInfo? userBudgetRoleInfo) =>
		userBudgetRoleInfo?.UserRole == UserRole.BudgetOwner;
}