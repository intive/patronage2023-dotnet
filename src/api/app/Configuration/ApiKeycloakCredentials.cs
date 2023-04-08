namespace Intive.Patronage2023.Api.Configuration;

/// <summary>
/// Api Keycloak Credentials - It is used to store the secret value required to authenticate with the Keycloak API.
/// </summary>
public class ApiKeycloakCredentials
{
	/// <summary>
	/// Represents the secret associated with the API Keycloak client.
	/// </summary>
	public string? Secret { get; set; }
}