using Intive.Patronage2023.Modules.User.Application.Extensions;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;

namespace Intive.Patronage2023.Modules.User.Application.RefreshingUserToken;
/// <summary>
/// Record which holds user refresh token.
/// </summary>
/// <param name="RefreshToken">refresh token.</param>
public record RefreshUserToken(string RefreshToken) : IQuery<AccessUserToken>;

/// <summary>
/// Record which holds newly generated access token.
/// </summary>
/// <param name="AccessToken"> newly generated access token.</param>
public record AccessUserToken(string AccessToken);

/// <summary>
/// refresh token command handler.
/// </summary>
public class HandleRefreshUserToken : IQueryHandler<RefreshUserToken, AccessUserToken>
{
	private readonly IKeycloakService keycloakService;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleRefreshUserToken"/> class.
	/// </summary>
	/// <param name="keycloakService">KeycloakService.</param>
	public HandleRefreshUserToken(IKeycloakService keycloakService) => this.keycloakService = keycloakService;

	/// <inheritdoc/>
	public async Task<AccessUserToken> Handle(RefreshUserToken command, CancellationToken cancellationToken)
	{
		string oldAccessToken = await this.keycloakService.ExtractAccessTokenFromClientToken(cancellationToken);
		string response = string.Empty;

		if (command.RefreshToken.IsExpired())
		{
			throw new AppException("refresh token is expired.");
		}

		if (oldAccessToken.IsExpired(TimeSpan.FromMinutes(-1)))
		{
			response = await this.keycloakService.RefreshUserToken(command.RefreshToken, cancellationToken);
		}

		return new AccessUserToken(response);
	}
}