using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Intive.Patronage2023.Modules.Budget.Api.ResourcePermissions;

/// <summary>
/// This class defines authorization requirements for different operations on a budget.
/// </summary>
public static class Operations
{
	/// <summary>
	/// Represents the "create" operation authorization requirement.
	/// </summary>
	public static readonly OperationAuthorizationRequirement Create =
		new OperationAuthorizationRequirement { Name = nameof(Create) };

	/// <summary>
	/// Represents the "read" operation authorization requirement.
	/// </summary>
	public static readonly OperationAuthorizationRequirement Read =
		new OperationAuthorizationRequirement { Name = nameof(Read) };

	/// <summary>
	/// Represents the "update" operation authorization requirement.
	/// </summary>
	public static readonly OperationAuthorizationRequirement Update =
		new OperationAuthorizationRequirement { Name = nameof(Update) };
}