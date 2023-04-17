using Intive.Patronage2023.Api.Configuration;
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
}