using System.IdentityModel.Tokens.Jwt;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.UserContext;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Shared.Infrastructure;

/// <summary>
/// Implementation of IExecutionContextAccessor which uses JWT token to
/// obtain required informations e.g. GetUserId uses "sub" claim to retrieve id.
/// </summary>
public class ExecutionContextAccessor : IExecutionContextAccessor
{
	private readonly IHttpContextAccessor httpContextAccessor;

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

	/// <summary>
	/// Returns information that user is admin or not.
	/// </summary>
	/// <returns>Bool value.</returns>
	public bool IsAdmin()
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

		return isAdmin;
	}

	/// <inheritdoc/>
	public UserInfo? GetUserContext()
	{
		string? jwtToken = this.httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
		var tokenHandler = new JwtSecurityTokenHandler();

		var token = tokenHandler.ReadJwtToken(jwtToken);

		if (token == null || token.Claims.All(c => c.Type != "sub"))
		{
			// JWT token is invalid
			return null;
		}

		var claims = token.Claims;
		var claimsDictionary = claims
			.ToDictionary(c => c.Type, c => c.Value);
		var userId = Guid.Parse(token.Claims.First(c => c.Type == "sub").Value);
		string userAvatar = string.Empty;
		if (claimsDictionary.ContainsKey("avatar"))
		{
			userAvatar = claimsDictionary["avatar"];
		}

		return new UserInfo
		{
			Id = userId,
			FirstName = claimsDictionary?["given_name"] ?? string.Empty,
			LastName = claimsDictionary?["family_name"] ?? string.Empty,
			Email = claimsDictionary?["email"] ?? string.Empty,
			Attributes = new UserAttributes
			{
				Avatar = new string[] { userAvatar },
			},
		};
	}
}