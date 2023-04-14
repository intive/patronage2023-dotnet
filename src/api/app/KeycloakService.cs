namespace Intive.Patronage2023.Api;

/// <summary>
/// KeycloakService.
/// </summary>
public class KeycloakService
{
	private readonly HttpClient httpClient;
	private readonly IConfiguration configuration;
	private readonly IHttpClientFactory httpClientFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="KeycloakService"/> class.
	/// KeycloakService.
	/// </summary>
	/// <param name="httpClient">HttpClient.</param>
	/// <param name="configuration">IConfiguration.</param>
	/// <param name="httpClientFactory">IHttpClientFactory.</param>
	public KeycloakService(HttpClient httpClient, IConfiguration configuration, IHttpClientFactory httpClientFactory)
	{
		this.httpClient = httpClient;
		this.configuration = configuration;
		this.httpClientFactory = httpClientFactory;

		var apiKeycloakSettings = this.configuration.GetSection("Keycloak").GetChildren();
		if (apiKeycloakSettings == null)
		{
			return;
		}

		string? authServerUrl = apiKeycloakSettings.FirstOrDefault(x => x.Key == "auth-server-url")?.Value;
		if (authServerUrl != null)
		{
			httpClient.BaseAddress = new Uri(authServerUrl);
			httpClient.Timeout = new TimeSpan(0, 0, 30);
		}
	}

	/// <summary>
	/// GetHttpClient.
	/// </summary>
	/// <returns>HttpClient.</returns>
	public HttpClient CreateClient()
	{
		var httpClient = this.httpClientFactory.CreateClient("ApiKeycloakClient");
		httpClient.BaseAddress = this.httpClient.BaseAddress;
		return httpClient;

		////var httpClient = new HttpClient { BaseAddress = this.httpClient.BaseAddress };
		////httpClient.DefaultRequestHeaders.Add("User-Agent", "ApiKeycloakClient");
		////return httpClient;
	}
}