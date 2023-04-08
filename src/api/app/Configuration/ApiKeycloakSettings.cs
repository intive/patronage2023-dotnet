namespace Intive.Patronage2023.Api.Configuration;

/// <summary>
/// Api Keycloak Settings -  It defines various properties which can be used to configure the Keycloak API settings required to connect to and authenticate with Keycloak.
/// </summary>
public class ApiKeycloakSettings
{
	/// <summary>
	/// The Realm name in Keycloak.
	/// </summary>
	public string? Realm { get; set; }

	/// <summary>
	/// The URL of the Keycloak authentication server.
	/// </summary>
	public string? AuthServerUrl { get; set; }

	/// <summary>
	/// Determines whether the audience of a token should be verified or not.
	/// </summary>
	public bool VerifyTokenAudience { get; set; }

	/// <summary>
	/// Represents whether SSL is required to connect to Keycloak or not.
	/// </summary>
	public string? SslRequired { get; set; }

	/// <summary>
	/// Represents the client id in Keycloak.
	/// </summary>
	public string? Resource { get; set; }

	/// <summary>
	/// An instance of the ApiKeycloakCredentials class which stores the secret value required to authenticate with the Keycloak API.
	/// </summary>
	public ApiKeycloakCredentials? Credentials { get; set; }

	/// <summary>
	/// Determines whether the client is public or not.
	/// </summary>
	public bool PublicClient { get; set; }

	/// <summary>
	/// Represents the port number of the Keycloak confidential client.
	/// </summary>
	public int ConfidentialPort { get; set; }
}