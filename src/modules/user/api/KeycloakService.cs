using System.Net.Http.Headers;

using Intive.Patronage2023.Modules.User.Api.Configuration;
using Intive.Patronage2023.Modules.User.Domain;
using Intive.Patronage2023.Modules.User.Infrastructure;

using Microsoft.Extensions.Options;

namespace Intive.Patronage2023.Modules.User.Api;

/// <summary>
/// Class KeycloakService.
/// </summary>
public class KeycloakService : IKeycloakService
{
	private readonly HttpClient httpClient;
	private readonly ApiKeycloakSettings apiKeycloakSettings;

	/// <summary>
	/// Initializes a new instance of the <see cref="KeycloakService"/> class.
	/// </summary>
	/// <param name="apiKeycloakSettings">ApiKeycloakSettings.</param>
	/// <param name="httpClient">HttpClient.</param>
	public KeycloakService(IOptions<ApiKeycloakSettings> apiKeycloakSettings, HttpClient httpClient)
	{
		this.apiKeycloakSettings = apiKeycloakSettings.Value;
		this.httpClient = httpClient;

		string authServerUrl = this.apiKeycloakSettings.AuthServerUrl;
		if (authServerUrl != null)
		{
			httpClient.BaseAddress = new Uri(authServerUrl);
			httpClient.Timeout = new TimeSpan(0, 0, 30);
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
		string resource = this.apiKeycloakSettings.Resource;
		string realm = this.apiKeycloakSettings.Realm;
		string secret = this.apiKeycloakSettings.Credentials.Secret;

		string url = $"/realms/{realm}/protocol/openid-connect/token";

		var content = new FormUrlEncodedContent(new[]
		{
				new KeyValuePair<string, string>("username", email),
				new KeyValuePair<string, string>("password", password),
				new KeyValuePair<string, string>("client_id", resource),
				new KeyValuePair<string, string>("client_secret", secret),
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
		string resource = this.apiKeycloakSettings.Resource;
		string realm = this.apiKeycloakSettings.Realm;
		string secret = this.apiKeycloakSettings.Credentials.Secret;

		string url = $"/realms/{realm}/protocol/openid-connect/token";

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("client_id", resource),
			new KeyValuePair<string, string>("client_secret", secret),
			new KeyValuePair<string, string>("grant_type", "client_credentials"),
		});

		return await this.httpClient.PostAsync(url, content, cancellationToken);
	}

	/// <summary>
	/// Add new user to keycloak.
	/// </summary>
	/// <param name="appUser">User to add.</param>
	/// <param name="accessToken">Client token.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage with JSON Web Token.</returns>
	public async Task<HttpResponseMessage> AddUser(AppUser appUser, string accessToken, CancellationToken cancellationToken)
	{
		string realm = this.apiKeycloakSettings.Realm;

		string url = $"/admin/realms/{realm}/users";

		this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		return await this.httpClient.PostAsJsonAsync(url, appUser, cancellationToken);
	}

	/// <summary>
	/// Get users from keycloak.
	/// </summary>
	/// <param name="searchText">Search text(string contained in username, first or last name, or email.</param>
	/// <param name="accessToken">Client token.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>List of users corresponding to query.</returns>
	public async Task<HttpResponseMessage> GetUsers(string? searchText, string accessToken, CancellationToken cancellationToken)
	{
		string realm = this.apiKeycloakSettings.Realm;

		string url = $"/admin/realms/{realm}/users";

		if (!string.IsNullOrEmpty(searchText))
		{
			url += $"?search={searchText.Trim()}";
		}

		this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		return await this.httpClient.GetAsync(url, cancellationToken);
	}
}