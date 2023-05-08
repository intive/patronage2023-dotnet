using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Intive.Patronage2023.Modules.Budget.Api;

/// <summary>
/// Operations.
/// </summary>
public static class Operations
{
	/// <summary>
	/// create.
	/// </summary>
	public static readonly OperationAuthorizationRequirement Create =
		new OperationAuthorizationRequirement { Name = nameof(Create) };

	/// <summary>
	/// read.
	/// </summary>
	public static readonly OperationAuthorizationRequirement Read =
		new OperationAuthorizationRequirement { Name = nameof(Read) };

	/// <summary>
	/// update.
	/// </summary>
	public static readonly OperationAuthorizationRequirement Update =
		new OperationAuthorizationRequirement { Name = nameof(Update) };
}