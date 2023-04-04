using System.Runtime.Serialization;

namespace Intive.Patronage2023.Modules.Example.Application.User.Commands;

/// <summary>
/// text.
/// </summary>
[DataContract]
public class Token
{
	/// <summary>
	/// AccessToken.
	/// </summary>
	[DataMember(Name = "access_token")]
	public string? AccessToken { get; set; }

	/// <summary>
	/// ExpiresIn.
	/// </summary>
	[DataMember(Name = "expires_in")]
	public int? ExpiresIn { get; set; }

	/// <summary>
	/// RefreshExpiresIn.
	/// </summary>
	[DataMember(Name = "refresh_expires_in")]
	public int? RefreshExpiresIn { get; set; }

	/// <summary>
	/// RefreshToken.
	/// </summary>
	[DataMember(Name = "refresh_token")]
	public string? RefreshToken { get; set; }

	/// <summary>
	/// TokenType.
	/// </summary>
	[DataMember(Name = "token_type")]
	public string? TokenType { get; set; }

	/// <summary>
	/// NotBeforePolicy.
	/// </summary>
	[DataMember(Name = "not-before-policy")]
	public int? NotBeforePolicy { get; set; }

	/// <summary>
	/// SessionState.
	/// </summary>
	[DataMember(Name = "session_state")]
	public string? SessionState { get; set; }

	/// <summary>
	/// Scope.
	/// </summary>
	[DataMember(Name = "scope")]
	public string? Scope { get; set; }
}