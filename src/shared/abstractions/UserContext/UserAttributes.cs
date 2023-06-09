namespace Intive.Patronage2023.Shared.Abstractions.UserContext;

/// <summary>
/// Additional informations about user.
/// </summary>
public class UserAttributes
{
	/// <summary>
	/// User avatar identifier.
	/// </summary>
	public string[] Avatar { get; set; } = null!;
}