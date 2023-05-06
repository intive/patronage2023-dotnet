using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Intive.Patronage2023.Modules.Budget.Api.ResourcePermissions;

/// <summary>
/// asdfasdf.
/// </summary>
public class BudgetAuthorizationHandler :
	AuthorizationHandler<OperationAuthorizationRequirement, GetBudgets>
{
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
		if (requirement.Name == Operations.Read.Name)
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}