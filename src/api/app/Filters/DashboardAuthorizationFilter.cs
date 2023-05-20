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
	private const string Role = "admin";
	private static readonly string HangFireCookieName = "HangFireCookie";
	private static readonly int CookieExpirationMinutes = 60;
	private readonly ILogger logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="DashboardAuthorizationFilter"/> class.
	/// </summary>
	/// <param name="logger">Logger.</param>
	public DashboardAuthorizationFilter(ILogger<DashboardAuthorizationFilter> logger)
	{
		this.logger = logger;
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
			if (!realmAccessValue.Contains(Role))
			{
				return false;
			}
		}
		catch (Exception e)
		{
			this.logger.LogError("Something went wrong: {E}", e);
			return false;
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