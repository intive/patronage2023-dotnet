using System.IdentityModel.Tokens.Jwt;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Shared.Infrastructure;

/// <summary>
/// Implementation of IExecutionContextAccessor which uses JWT token to
/// obtain required informations e.g. GetUserId uses "sub" claim to retrieve id.
/// </summary>
public class ExecutionContextAccessor : IExecutionContextAccessor
{
	private IHttpContextAccessor httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExecutionContextAccessor"/> class.
	/// </summary>
	/// <param name="httpContextAccessor">Http context accessor provided by DI.</param>
	public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) =>
	this.httpContextAccessor = httpContextAccessor;

	/// <inheritdoc />
	public Guid? GetUserId()
	{
		string? jwtToken = this.httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
		if (jwtToken == null)
		{
			// User is not authenticated
			return null;
		}

		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.ReadJwtToken(jwtToken);

		if (token == null || token.Claims.All(c => c.Type != "sub"))
		{
			// JWT token is invalid
			return null;
		}

		var userId = Guid.Parse(token.Claims.First(c => c.Type == "sub").Value);
		return userId;
	}

	/// <inheritdoc/>
	public bool IsUserAdmin()
	{
		string? jwtToken = this.httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
		if (jwtToken == null)
		{
			// User is not authenticated
			return false;
		}

		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.ReadJwtToken(jwtToken);

		string realmAccessValue = token.Claims.First(c => c.Type == "realm_access").Value;
		bool isAdmin = realmAccessValue.Contains("admin");

		if (isAdmin)
		{
			return true;
		}

		return false;
	}
}