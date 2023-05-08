using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

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
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetAuthorizationHandler"/> class.
	/// BudgetAuthorizationHandler.
	/// </summary>
	/// <param name="contextAccessor">.</param>
	/// <param name="budgetDbContext">Instance used to access the database.</param>
	public BudgetAuthorizationHandler(IExecutionContextAccessor contextAccessor, BudgetDbContext budgetDbContext)
	{
		this.contextAccessor = contextAccessor;
		this.budgetDbContext = budgetDbContext;
	}

	private BudgetId BudgetId { get; set; }

	private UserId UserId { get; set; }

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

		this.UserId = new UserId(this.contextAccessor.GetUserId()!.Value);
		this.BudgetId = budgetId;

		if (!await this.IsUserContributor())
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
				if (!await this.IsUserBudgetOwner())
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

	private async Task<bool> IsUserContributor() =>
		await this.budgetDbContext.UserBudget
			.AnyAsync(x => x.UserId == this.UserId && x.BudgetId == this.BudgetId);

	private async Task<UserRole> GetUserRoleForBudget() =>
		await this.budgetDbContext.UserBudget
		.Where(x => x.UserId == this.UserId && x.BudgetId == this.BudgetId)
		.Select(x => x.UserRole)
		.FirstAsync();

	private async Task<bool> IsUserBudgetOwner() =>
		await this.GetUserRoleForBudget() == UserRole.BudgetOwner;
}