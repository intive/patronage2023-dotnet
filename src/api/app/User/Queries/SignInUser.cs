using Intive.Patronage2023.Api.Configuration;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.Extensions.Options;

namespace Intive.Patronage2023.Api;

/// <summary>
/// SignIn user query.
/// </summary>
/// <param name="Username">Username which user provides.</param>
/// <param name="Password">Password which user provides.</param>
public record SignInUser(string Username, string Password) : IQuery<HttpResponseMessage>;

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
	private readonly IHttpClientFactory httpClientFactory;
	private readonly ApiKeycloakSettings apiKeycloakSettings;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSignInUser"/> class.
	/// </summary>
	/// <param name="httpClientFactory">IHttpClientFactory.</param>
	/// <param name="apiKeycloakSettings">ApiKeycloakSettings.</param>
	public HandleSignInUser(IHttpClientFactory httpClientFactory, IOptions<ApiKeycloakSettings> apiKeycloakSettings)
	{
		this.httpClientFactory = httpClientFactory;
		this.apiKeycloakSettings = apiKeycloakSettings.Value;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSignInUser"/> class.
	/// </summary>
	/// <param name="request">request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage with JSON Web Token .</returns>
	public async Task<HttpResponseMessage> Handle(SignInUser request, CancellationToken cancellationToken)
	{
		var httpClient = this.httpClientFactory.CreateClient();
		string? resource = this.apiKeycloakSettings.Resource;
		string? secret = this.apiKeycloakSettings.Credentials?.Secret;
		string? realm = this.apiKeycloakSettings.Realm;
		string? url = $"http://localhost:8080/realms/{realm}/protocol/openid-connect/token";

		var content = new FormUrlEncodedContent(new[]
		{
				new KeyValuePair<string, string>("username", request.Username),
				new KeyValuePair<string, string>("password", request.Password),
				new KeyValuePair<string, string>("client_id", resource ?? string.Empty),
				new KeyValuePair<string, string>("client_secret", secret ?? string.Empty),
				new KeyValuePair<string, string>("grant_type", "password"),
		});

		var response = await httpClient.PostAsync(url, content);

		return response;
	}
}