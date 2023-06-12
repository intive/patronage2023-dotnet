namespace Intive.Patronage2023.Shared.Abstractions.UserContext;

/// <summary>
/// User information.
/// </summary>
public record UserInfoDto() : UserInfoBase
{
	/// <summary>
	/// User Avatar.
	/// </summary>
	public string? Avatar { get; set; }
}