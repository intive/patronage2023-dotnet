using System.Runtime.Serialization;

namespace Intive.Patronage2023.Api.Configuration;

/// <summary>
/// Api Keycloak Settings -  It defines various properties which can be used to configure the Keycloak API settings required to connect to and authenticate with Keycloak.
/// </summary>
[DataContract]
public class ApiKeycloakSettings
{
	/// <summary>
	/// The Realm name in Keycloak.
	/// </summary>
	[DataMember(Name = "realm")]
	public string? Realm { get; set; }

	/// <summary>
	/// The URL of the Keycloak authentication server.
	/// </summary>
	[DataMember(Name = "auth-server-url")]
	public string? AuthServerUrl { get; set; }

	/// <summary>
	/// Determines whether the audience of a token should be verified or not.
	/// </summary>
	[DataMember(Name = "verify-token-audience")]
	public bool VerifyTokenAudience { get; set; }

	/// <summary>
	/// Represents whether SSL is required to connect to Keycloak or not.
	/// </summary>
	[DataMember(Name = "ssl-required")]
	public string? SslRequired { get; set; }

	/// <summary>
	/// Represents the client id in Keycloak.
	/// </summary>
	[DataMember(Name = "resource")]
	public string? Resource { get; set; }

	/// <summary>
	/// Determines whether the client is public or not.
	/// </summary>
	[DataMember(Name = "public-client")]
	public bool PublicClient { get; set; }

	/// <summary>
	/// Represents the port number of the Keycloak confidential client.
	/// </summary>
	[DataMember(Name = "confidential-port")]
	public int ConfidentialPort { get; set; }
}