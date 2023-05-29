using System.Net.Http.Headers;
using Intive.Patronage2023.Modules.User.Api.Configuration;
using Intive.Patronage2023.Modules.User.Domain;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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

	/// <inheritdoc/>
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

	/// <inheritdoc/>
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

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> AddUser(AppUser appUser, CancellationToken cancellationToken)
	{
		string accessToken = await this.ExtractAccessTokenFromClientToken(cancellationToken);

		string realm = this.apiKeycloakSettings.Realm;

		string url = $"/admin/realms/{realm}/users";

		this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		return await this.httpClient.PostAsJsonAsync(url, appUser, cancellationToken);
	}

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> GetUsers(string? searchText, CancellationToken cancellationToken)
	{
		string accessToken = await this.ExtractAccessTokenFromClientToken(cancellationToken);

		string realm = this.apiKeycloakSettings.Realm;

		string url = $"/admin/realms/{realm}/users";

		if (!string.IsNullOrEmpty(searchText))
		{
			url += $"?search={searchText.Trim()}";
		}

		this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		return await this.httpClient.GetAsync(url, cancellationToken);
	}

	/// <inheritdoc/>
	public async Task<string> ExtractAccessTokenFromClientToken(CancellationToken cancellationToken)
	{
		var response = await this.GetClientToken(cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException();
		}

		string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

		if (string.IsNullOrEmpty(responseContent))
		{
			throw new AppException();
		}

		Token? token = JsonConvert.DeserializeObject<Token>(responseContent);

		if (token == null || token?.AccessToken == null)
		{
			throw new AppException();
		}

		return token.AccessToken;
	}

	/// <inheritdoc/>
	public async Task<HttpResponseMessage> GetUserById(string id, CancellationToken cancellationToken)
	{
		string accessToken = await this.ExtractAccessTokenFromClientToken(cancellationToken);

		string realm = this.apiKeycloakSettings.Realm;

		string url = $"/admin/realms/{realm}/users/{id}";

		this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		return await this.httpClient.GetAsync(url, cancellationToken);
	}
}