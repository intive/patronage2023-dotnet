using System.IdentityModel.Tokens.Jwt;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Shared.Infrastructure;

/// <summary>
/// Implementation of IExecutionContextAccessor.
/// </summary>
public class ExecutionContextAccessor : IExecutionContextAccessor
{
	/// <inheritdoc />
	public Guid? GetUserId(HttpContext httpContext)
	{
		string? jwtToken = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
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
}