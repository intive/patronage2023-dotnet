namespace Intive.Patronage2023.Api.Configuration;

/// <summary>
/// ApiKeycloakSettings.
/// </summary>
public class ApiKeycloakSettings
{
	/// <summary>
	/// Realm.
	/// </summary>
	public string? Realm { get; set; }

	/// <summary>
	/// AuthServerUrl.
	/// </summary>
	public string? AuthServerUrl { get; set; }

	/// <summary>
	/// VerifyTokenAudience.
	/// </summary>
	public bool VerifyTokenAudience { get; set; }

	/// <summary>
	/// SslRequired.
	/// </summary>
	public string? SslRequired { get; set; }

	/// <summary>
	/// Resource.
	/// </summary>
	public string? Resource { get; set; }

	/// <summary>
	/// Credentials.
	/// </summary>
	public ApiKeycloakCredentials? Credentials { get; set; }

	/// <summary>
	/// PublicClient.
	/// </summary>
	public bool PublicClient { get; set; }

	/// <summary>
	/// ConfidentialPort.
	/// </summary>
	public int ConfidentialPort { get; set; }
}