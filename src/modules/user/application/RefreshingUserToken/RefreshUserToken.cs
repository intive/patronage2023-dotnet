using System.IdentityModel.Tokens.Jwt;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;

namespace Intive.Patronage2023.Modules.User.Application.RefreshingUserToken;
/// <summary>
/// Record which holds user refresh token.
/// </summary>
/// <param name="RefreshToken">refresh token.</param>
public record RefreshUserToken(string RefreshToken) : IQuery<AccesToken>;

/// <summary>
/// Record which holds newly generated access token.
/// </summary>
/// <param name="AccessToken"> newly generated access token.</param>
public record AccesToken(string AccessToken);

/// <summary>
/// refresh token command handler.
/// </summary>
public class HandleRefreshUserToken : IQueryHandler<RefreshUserToken, AccesToken>
{
	private readonly IKeycloakService keycloakService;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleRefreshUserToken"/> class.
	/// </summary>
	/// <param name="keycloakService">KeycloakService.</param>
	public HandleRefreshUserToken(IKeycloakService keycloakService) => this.keycloakService = keycloakService;

	/// <inheritdoc/>
	public async Task<AccesToken> Handle(RefreshUserToken command, CancellationToken cancellationToken)
	{
		string oldAccessToken = await this.keycloakService.ExtractAccessTokenFromClientToken(cancellationToken);
		string response = string.Empty;

		var jwtHandler = new JwtSecurityTokenHandler();

		var accessToken = jwtHandler.ReadJwtToken(oldAccessToken);
		var refreshToken = jwtHandler.ReadJwtToken(command.RefreshToken);

		if (refreshToken.ValidTo < DateTime.UtcNow)
		{
			throw new AppException("refresh token is expired.");
		}

		if (accessToken.ValidTo.AddMinutes(-1) <= DateTime.UtcNow)
		{
			response = await this.keycloakService.RefreshUserToken(command.RefreshToken, cancellationToken);
		}

		return new AccesToken(response);
	}
}