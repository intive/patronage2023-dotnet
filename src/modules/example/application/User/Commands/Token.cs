using System.Runtime.Serialization;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using MediatR;

namespace Intive.Patronage2023.Modules.Example.Application.User.Commands;

/// <summary>
/// SignIn command.
/// </summary>
/// <param name="Username">User name.</param>
/// <param name="Password">User password.</param>
public record SignInCommand(string Username, string Password) : ICommand;

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