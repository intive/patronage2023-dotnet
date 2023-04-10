using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// Interface to retrive user id from HttpContext.
/// </summary>
public interface IExecutionContextAccessor
{
	/// <summary>
	/// Returns user id extraced from token.
	/// </summary>
	/// <param name="httpContext">Context to retrive token from.</param>
	/// <returns>User id or null if token/claim does not exits.</returns>
	Guid? GetUserId(HttpContext httpContext);
}