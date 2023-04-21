namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// Interface to retrive user id from HttpContext.
/// </summary>
public interface IExecutionContextAccessor
{
	/// <summary>
	/// Returns user id extraced from token.
	/// </summary>
	/// <returns>User id or null if token/claim does not exits.</returns>
	Guid? GetUserId();
}