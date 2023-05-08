using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Api.ResourcePermissions;

/// <summary>
/// Test Authorization Class.
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

	/// <summary>
	/// asdfasdf.
	/// </summary>
	/// <param name="context">asdfassdf.</param>
	/// <param name="requirement">qweqweqwe.</param>
	/// <param name="budgetId">qweqaweqwe.</param>
	/// <returns>afsdfasdfasdf.</returns>
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

		var userId = new UserId(this.contextAccessor.GetUserId()!.Value);

		bool isPermissions = await this.budgetDbContext.UserBudget.AnyAsync(x => x.UserId == userId && x.BudgetId == budgetId);
		var roles = await this.budgetDbContext.UserBudget
			.Where(x => x.UserId == userId && x.BudgetId == budgetId)
			.Select(x => x.UserRole)
			.FirstAsync();

		if (!isPermissions)
		{
			context.Fail();
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

				if (roles != UserRole.BudgetOwner)
				{
					context.Fail();
				}

				context.Succeed(requirement);
				break;

			default:
				throw new ArgumentException("Unknown permission requirement.", nameof(requirement));
		}
	}
}