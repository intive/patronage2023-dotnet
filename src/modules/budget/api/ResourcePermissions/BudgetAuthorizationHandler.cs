using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Intive.Patronage2023.Modules.Budget.Api.ResourcePermissions;

/// <summary>
/// Test Authorization Class.
/// </summary>
public class BudgetAuthorizationHandler :
	AuthorizationHandler<OperationAuthorizationRequirement, GetBudgets>
{
	private readonly IExecutionContextAccessor contextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetAuthorizationHandler"/> class.
	/// BudgetAuthorizationHandler.
	/// </summary>
	/// <param name="contextAccessor">.</param>
	public BudgetAuthorizationHandler(IExecutionContextAccessor contextAccessor)
	{
		this.contextAccessor = contextAccessor;
	}

	/// <summary>
	/// asdfasdf.
	/// </summary>
	/// <param name="context">asdfassdf.</param>
	/// <param name="requirement">qweqweqwe.</param>
	/// <param name="resource">qweqaweqwe.</param>
	/// <returns>afsdfasdfasdf.</returns>
	protected override Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		OperationAuthorizationRequirement requirement,
		GetBudgets resource)
	{
		////bool isAdmin = this.contextAccessor.IsUserAdmin();
		////var userId = this.contextAccessor.GetUserId();

		////if (isAdmin)
		////{
		////	return Task.CompletedTask;
		////}

		if (requirement.Name == Operations.Read.Name)
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}