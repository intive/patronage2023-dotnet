using Intive.Patronage2023.Modules.User.Application.Extensions;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

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
	private readonly IHttpContextAccessor httpContextAccessor;
	private readonly IKeycloakService keycloakService;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleRefreshUserToken"/> class.
	/// </summary>
	/// <param name="keycloakService">.</param>
	/// <param name="httpContextAccessor">KeycloakService.</param>
	public HandleRefreshUserToken(IKeycloakService keycloakService, IHttpContextAccessor httpContextAccessor)
	{
		this.keycloakService = keycloakService;
		this.httpContextAccessor = httpContextAccessor;
	}

	/// <inheritdoc/>
	public async Task<AccessUserToken> Handle(RefreshUserToken command, CancellationToken cancellationToken)
	{
		string? oldAccessToken = this.httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

		string response = string.Empty;

		if (command.RefreshToken.IsExpired())
		{
			throw new AppException("refresh token is expired.");
		}

		if (oldAccessToken!.IsExpired(TimeSpan.FromMinutes(-1)))
		{
			response = await this.keycloakService.RefreshUserToken(command.RefreshToken, cancellationToken);
		}

		return new AccessUserToken(response);
	}
}