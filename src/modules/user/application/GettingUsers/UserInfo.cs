using Intive.Patronage2023.Modules.User.Contracts;

namespace Intive.Patronage2023.Modules.User.Application.GettingUsers;

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