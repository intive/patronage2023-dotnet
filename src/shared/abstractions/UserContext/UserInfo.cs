namespace Intive.Patronage2023.Shared.Abstractions.UserContext;

/// <summary>
/// User information.
/// </summary>
public record UserInfo() : UserInfoBase
{
	/// <summary>
	/// User additional information.
	/// </summary>
	public UserAttributes Attributes { get; set; } = null!;
}