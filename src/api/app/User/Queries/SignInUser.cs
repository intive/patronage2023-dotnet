using Intive.Patronage2023.Api.Configuration;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.Extensions.Options;

namespace Intive.Patronage2023.Api.User;

/// <summary>
/// SignIn user query.
/// </summary>
/// <param name="Email">User email which user provides.</param>
/// <param name="Password">Password which user provides.</param>
public record SignInUser(string Email, string Password) : IQuery<HttpResponseMessage>;

/// <summary>
/// Handles a sign-in request from a user.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IQueryHandler{TQuery,TResult}"/> interface to handle a sign-in request
/// from a user. It sends the sign-in request to the authentication service (Keycloak) and returns an HTTP response that
/// includes a JWT token if the sign-in was successful.
/// </remarks>
public class HandleSignInUser : IQueryHandler<SignInUser, HttpResponseMessage>
{
	private readonly ApiKeycloakSettings apiKeycloakSettings;
	private readonly KeycloakService keycloakService;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSignInUser"/> class.
	/// </summary>
	/// <param name="apiKeycloakSettings">ApiKeycloakSettings.</param>
	/// <param name="keycloakService">KeycloakService.</param>
	public HandleSignInUser(IOptions<ApiKeycloakSettings> apiKeycloakSettings, KeycloakService keycloakService)
	{
		this.apiKeycloakSettings = apiKeycloakSettings.Value;
		this.keycloakService = keycloakService;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSignInUser"/> class.
	/// </summary>
	/// <param name="request">request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage with JSON Web Token.</returns>
	public async Task<HttpResponseMessage> Handle(SignInUser request, CancellationToken cancellationToken)
	{
		HttpClient httpClient = await this.keycloakService.CreateClientAsync();
		string? resource = this.apiKeycloakSettings.Resource;
		string? realm = this.apiKeycloakSettings.Realm;
		string? url = $"/realms/{realm}/protocol/openid-connect/token";

		var content = new FormUrlEncodedContent(new[]
		{
				new KeyValuePair<string, string>("username", request.Email),
				new KeyValuePair<string, string>("password", request.Password),
				new KeyValuePair<string, string>("client_id", resource ?? string.Empty),
				new KeyValuePair<string, string>("grant_type", "password"),
		});

		var response = await httpClient.PostAsync(url, content, cancellationToken);

		return response;
	}
}