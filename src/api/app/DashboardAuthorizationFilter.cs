using Hangfire.Dashboard;

namespace Intive.Patronage2023.Api;

/// <summary>
/// DashboardAuthorizationFilter is a class that implements the IDashboardAuthorizationFilter interface.
/// It provides authorization logic for the Hangfire Dashboard.
/// </summary>
public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
	/// <summary>
	/// Determines whether the current request is authorized to access the Hangfire Dashboard.
	/// </summary>
	/// <param name="context">The DashboardContext representing the current request.</param>
	/// <returns>True if the request is authorized; otherwise, false.</returns>
	public bool Authorize(DashboardContext context)
	{
		return true;
	}
}