namespace Intive.Patronage2023.Api;

/// <summary>
/// Class KeycloakService.
/// </summary>
public class KeycloakService
{
	private readonly IConfiguration configuration;
	private readonly IHttpClientFactory httpClientFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="KeycloakService"/> class.
	/// </summary>
	/// <param name="configuration">IConfiguration.</param>
	/// <param name="httpClientFactory">IHttpClientFactory.</param>
	public KeycloakService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
	{
		this.configuration = configuration;
		this.httpClientFactory = httpClientFactory;
	}

	/// <summary>
	/// CreateClientAsync method.
	/// </summary>
	/// <returns>HttpClient object.</returns>
	public async Task<HttpClient> CreateClientAsync()
	{
		var apiKeycloakSettings = await Task.Run(() => this.configuration.GetSection("Keycloak").GetChildren());
		var httpClient = this.httpClientFactory.CreateClient("ApiKeycloakClient");

		if (apiKeycloakSettings != null)
		{
			string? authServerUrl = apiKeycloakSettings.FirstOrDefault(x => x.Key == "auth-server-url")?.Value;
			if (authServerUrl != null)
			{
				httpClient.BaseAddress = new Uri(authServerUrl);
				httpClient.Timeout = new TimeSpan(0, 0, 30);
			}
		}

		return httpClient;
	}
}