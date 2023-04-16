using System.Net.Http.Headers;
using Intive.Patronage2023.Api.Configuration;
using Intive.Patronage2023.Api.User.CreatingUser;
using Intive.Patronage2023.Api.User.Models;
using Microsoft.Extensions.Options;

namespace Intive.Patronage2023.Api.Keycloak;

/// <summary>
/// Class KeycloakService.
/// </summary>
public class KeycloakService
{
	private readonly HttpClient httpClient;
	private readonly ApiKeycloakSettings? apiKeycloakSettings;

	/// <summary>
	/// Initializes a new instance of the <see cref="KeycloakService"/> class.
	/// </summary>
	/// <param name="apiKeycloakSettings">ApiKeycloakSettings.</param>
	/// <param name="httpClient">HttpClient.</param>
	public KeycloakService(IOptions<ApiKeycloakSettings> apiKeycloakSettings, HttpClient httpClient)
	{
		this.apiKeycloakSettings = apiKeycloakSettings.Value;
		this.httpClient = httpClient;

		if (this.apiKeycloakSettings != null)
		{
			string? authServerUrl = this.apiKeycloakSettings.AuthServerUrl;
			if (authServerUrl != null)
			{
				httpClient.BaseAddress = new Uri(authServerUrl);
				httpClient.Timeout = new TimeSpan(0, 0, 30);
			}
		}
	}

	/// <summary>
	/// SignInGetToken method.
	/// </summary>
	/// <param name="email">User email.</param>
	/// <param name="password">User password.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage object.</returns>
	public async Task<HttpResponseMessage> SignInGetToken(string email, string password, CancellationToken cancellationToken)
	{
		string? resource = this.apiKeycloakSettings?.Resource;
		string? realm = this.apiKeycloakSettings?.Realm;
		string? url = $"/realms/{realm}/protocol/openid-connect/token";

		var content = new FormUrlEncodedContent(new[]
		{
				new KeyValuePair<string, string>("username", email),
				new KeyValuePair<string, string>("password", password),
				new KeyValuePair<string, string>("client_id", resource ?? string.Empty),
				new KeyValuePair<string, string>("grant_type", "password"),
		});

		return await this.httpClient.PostAsync(url, content, cancellationToken);
	}

	/// <summary>
	/// Get client token method.
	/// </summary>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage object.</returns>
	public async Task<HttpResponseMessage> GetClientToken(CancellationToken cancellationToken)
	{
		string? resource = this.apiKeycloakSettings?.Resource;
		string? realm = this.apiKeycloakSettings?.Realm;
		string? secret = this.apiKeycloakSettings?.Credentials?.Secret;

		string? url = $"/realms/{realm}/protocol/openid-connect/token";

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("client_id", resource ?? string.Empty),
			new KeyValuePair<string, string>("client_secret", secret ?? string.Empty),
			new KeyValuePair<string, string>("grant_type", "client_credentials"),
		});

		return await this.httpClient.PostAsync(url, content, cancellationToken);
	}

	/// <summary>
	/// Add new user to keycloak.
	/// </summary>
	/// <param name="createUser">User to add.</param>
	/// <param name="accessToken">Client token.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage with JSON Web Token.</returns>
	public async Task<HttpResponseMessage> AddUser(CreateUser createUser, string accessToken, CancellationToken cancellationToken)
	{
		string? realm = this.apiKeycloakSettings?.Realm;

		string? url = $"/admin/realms/{realm}/users";

		UserCredentials[] credentials =
		{
			new UserCredentials
			{
				Type = "password",
				Value = createUser.Password,
				Temporary = false,
			},
		};

		var attributes = new UserAttributes
		{
			Avatar = createUser.Avatar,
		};

		var content = new AppUser
		{
			Email = createUser.Email,
			FirstName = createUser.FirstName,
			LastName = createUser.LastName,
			Enabled = true,
			Attributes = attributes,
			Credentials = credentials,
		};

		this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		return await this.httpClient.PostAsJsonAsync(url, content, cancellationToken);
	}
}