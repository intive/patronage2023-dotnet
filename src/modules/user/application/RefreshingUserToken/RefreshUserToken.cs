using Intive.Patronage2023.Modules.User.Application.Extensions;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;

namespace Intive.Patronage2023.Modules.User.Application.RefreshingUserToken;
/// <summary>
/// Record which holds user refresh token.
/// </summary>
/// <param name="RefreshToken">refresh token.</param>
public record RefreshUserToken(string RefreshToken) : IQuery<Token>;

/// <summary>
/// refresh token command handler.
/// </summary>
public class HandleRefreshUserToken : IQueryHandler<RefreshUserToken, Token>
{
	private readonly IKeycloakService keycloakService;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleRefreshUserToken"/> class.
	/// </summary>
	/// <param name="keycloakService">Service that connects to Keycloak IDP.</param>
	public HandleRefreshUserToken(IKeycloakService keycloakService)
	{
		this.keycloakService = keycloakService;
	}

	/// <inheritdoc/>
	public async Task<Token> Handle(RefreshUserToken command, CancellationToken cancellationToken)
	{
		if (command.RefreshToken.IsExpired())
		{
			throw new AppException("refresh token is expired.");
		}

		var response = await this.keycloakService.RefreshUserToken(command.RefreshToken, cancellationToken);

		return response;
	}
}