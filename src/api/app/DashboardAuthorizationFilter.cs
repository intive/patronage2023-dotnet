using System.IdentityModel.Tokens.Jwt;
using Hangfire.Dashboard;

namespace Intive.Patronage2023.Api;

/// <summary>
/// DashboardAuthorizationFilter is a class that implements the IDashboardAuthorizationFilter interface.
/// It provides authorization logic for the Hangfire Dashboard.
/// </summary>
public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
	private static readonly string HangFireCookieName = "HangFireCookie";
	private static readonly int CookieExpirationMinutes = 60;
	private string role;

	/// <summary>
	/// Initializes a new instance of the <see cref="DashboardAuthorizationFilter"/> class.
	/// asdfasdf.
	/// </summary>
	public DashboardAuthorizationFilter()
	{
		this.role = "admin";
	}

	/// <summary>
	/// adfasdf.
	/// </summary>
	/// <param name="context">dd.</param>
	/// <returns>ff.</returns>
	/// <exception cref="Exception">hh.</exception>
	public bool Authorize(DashboardContext context)
	{
		var httpContext = context.GetHttpContext();

		string? access_token = string.Empty;
		bool setCookie = false;

		// try to get token from query string
		if (httpContext.Request.Query.ContainsKey("access_token"))
		{
			access_token = httpContext.Request.Query["access_token"].FirstOrDefault();
			setCookie = true;
		}
		else
		{
			access_token = httpContext.Request.Cookies[HangFireCookieName];
		}

		if (string.IsNullOrEmpty(access_token))
		{
			return false;
		}

		try
		{
			var hand = new JwtSecurityTokenHandler();
			var token = hand.ReadJwtToken(access_token);
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
				access_token,
				new CookieOptions()
				{
					Expires = DateTime.Now.AddMinutes(CookieExpirationMinutes),
				});
		}

		return true;
	}
}