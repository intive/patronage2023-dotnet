using System.IdentityModel.Tokens.Jwt;
using Hangfire.Dashboard;

namespace Intive.Patronage2023.Api.Filters;

/// <summary>
/// The DashboardAuthorizationFilter class is responsible for implementing the
/// IDashboardAuthorizationFilter interface and providing authorization logic
/// for the Hangfire Dashboard. It handles authentication and access control for the dashboard.
/// </summary>
public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
	private static readonly string HangFireCookieName = "HangFireCookie";
	private static readonly int CookieExpirationMinutes = 60;
	private string role;

	/// <summary>
	/// Initializes a new instance of the <see cref="DashboardAuthorizationFilter"/> class.
	/// </summary>
	public DashboardAuthorizationFilter()
	{
		this.role = "admin";
	}

	/// <summary>
	/// This method is called to authorize access to the Hangfire Dashboard.
	/// It receives the DashboardContext object containing the current context information.
	/// </summary>
	/// <param name="context">Object containing the current context information.</param>
	/// <returns>True if user is Admin, False otherwise.</returns>
	public bool Authorize(DashboardContext context)
	{
		var httpContext = context.GetHttpContext();

		string? accessToken = string.Empty;
		bool setCookie = false;

		// try to get token from query string
		if (httpContext.Request.Query.ContainsKey("access_token"))
		{
			accessToken = httpContext.Request.Query["access_token"].FirstOrDefault();
			setCookie = true;
		}
		else
		{
			accessToken = httpContext.Request.Cookies[HangFireCookieName];
		}

		if (string.IsNullOrEmpty(accessToken))
		{
			return false;
		}

		try
		{
			var hand = new JwtSecurityTokenHandler();
			var token = hand.ReadJwtToken(accessToken);
			string realmAccessValue = token.Claims.First(c => c.Type == "realm_access").Value;
			if (!string.IsNullOrEmpty(this.role) && !realmAccessValue.Contains(this.role))
			{
				return false;
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		if (setCookie)
		{
			httpContext.Response.Cookies.Append(
				HangFireCookieName,
				accessToken,
				new CookieOptions()
				{
					Expires = DateTime.Now.AddMinutes(CookieExpirationMinutes),
				});
		}

		return true;
	}
}