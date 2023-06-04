using System.IdentityModel.Tokens.Jwt;
using Intive.Patronage2023.Shared.Infrastructure;

namespace Intive.Patronage2023.Modules.User.Application.Extensions;

/// <summary>
/// Token extensions class.
/// </summary>
internal static class TokenQueryExtensions
{
	/// <summary>
	/// Method which checks if token is expired or not.
	/// </summary>
	/// <param name="token">Token to be checked.</param>
	/// <param name="timeSpan">time offset from the Valid time.</param>
	/// <returns>.</returns>
	public static bool IsExpired(this string token, TimeSpan timeSpan = default(TimeSpan))
	{
		var dateTimeProvider = new DateTimeProvider();
		var jwtHandler = new JwtSecurityTokenHandler();

		var tokenToBeValidated = jwtHandler.ReadJwtToken(token);

		if (tokenToBeValidated.ValidTo.AddMinutes(-timeSpan.TotalMinutes) < dateTimeProvider.UtcNow)
		{
			return true;
		}

		return false;
	}
}