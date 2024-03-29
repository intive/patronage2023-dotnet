using Intive.Patronage2023.Shared.Abstractions.UserContext;

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

	/// <summary>
	/// Returns information that user is admin or not.
	/// </summary>
	/// <returns>Bool value.</returns>
	bool IsAdmin();

	/// <summary>
	/// Holds information from token claims.
	/// </summary>
	/// <returns>Dictionary with mapped values from token.</returns>
	public UserInfo? GetUserContext();
}