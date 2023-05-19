namespace Intive.Patronage2023.Modules.User.Application.GettingUsers;

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