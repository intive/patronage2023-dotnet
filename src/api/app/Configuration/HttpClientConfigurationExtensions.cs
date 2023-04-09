namespace Intive.Patronage2023.Api.Configuration;

/// <summary>
/// Static class HttpClientConfigurationExtensions.
/// </summary>
public static class HttpClientConfigurationExtensions
{
	/// <summary>
	/// Extension method sets up an HttpClient instance by calling the AddHttpClient method
	/// of the service collection with the httpClientName and a lambda function that configures the HttpClient instance.
	/// </summary>
	/// <param name="services">IServiceCollection object.</param>
	/// <param name="configuration">IConfiguration object.</param>
	/// <param name="httpClientName">Name of the HttpClient.</param>
	public static void AddHttpClient(
		this IServiceCollection services, IConfiguration configuration, string httpClientName) => services.AddHttpClient(httpClientName, httpClient =>
		{
			var apiKeycloakSettings = configuration.GetSection("Keycloak").GetChildren();
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
		});
}