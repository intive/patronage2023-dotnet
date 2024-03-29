namespace Intive.Patronage2023.Modules.User.Api.Configuration;

/// <summary>
/// Api Keycloak Credentials - It is used to store the secret value required to authenticate with the Keycloak API.
/// </summary>
public class ApiKeycloakCredentials
{
	/// <summary>
	/// Represents the secret associated with the API Keycloak client.
	/// </summary>
	[ConfigurationKeyName("secret")]
	public string Secret { get; set; } = null!;
}