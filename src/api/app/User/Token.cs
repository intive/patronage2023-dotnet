using System.Runtime.Serialization;

namespace Intive.Patronage2023.Api.User;

/// <summary>
/// Class represents a JWT token used for authentication.
/// </summary>
[DataContract]
public class Token
{
	/// <summary>
	/// The access token value.
	/// </summary>
	[DataMember(Name = "access_token")]
	public string? AccessToken { get; set; }

	/// <summary>
	/// The number of seconds until the token expires.
	/// </summary>
	[DataMember(Name = "expires_in")]
	public int? ExpiresIn { get; set; }

	/// <summary>
	/// The number of seconds until the refresh token expires.
	/// </summary>
	[DataMember(Name = "refresh_expires_in")]
	public int? RefreshExpiresIn { get; set; }

	/// <summary>
	/// The refresh token value.
	/// </summary>
	[DataMember(Name = "refresh_token")]
	public string? RefreshToken { get; set; }

	/// <summary>
	/// The type of the token (e.g. "Bearer").
	/// </summary>
	[DataMember(Name = "token_type")]
	public string? TokenType { get; set; }

	/// <summary>
	/// The time (in seconds) before which the token is not valid.
	/// </summary>
	[DataMember(Name = "not-before-policy")]
	public int? NotBeforePolicy { get; set; }

	/// <summary>
	/// The session state value.
	/// </summary>
	[DataMember(Name = "session_state")]
	public string? SessionState { get; set; }

	/// <summary>
	/// The scopes associated with the token.
	/// </summary>
	[DataMember(Name = "scope")]
	public string? Scope { get; set; }
}