namespace Intive.Patronage2023.Modules.User.Contracts;

/// <summary>
/// User information about credentials.
/// </summary>
public class UserCredentials
{
	/// <summary>
	/// Credentials type.
	/// </summary>
	public string Type { get; set; } = null!;

	/// <summary>
	/// Credentials value.
	/// </summary>
	public string Value { get; set; } = null!;

	/// <summary>
	/// Value lifetime.
	/// </summary>
	public bool Temporary { get; set; }
}