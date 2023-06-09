using Intive.Patronage2023.Shared.Abstractions.UserContext;

namespace Intive.Patronage2023.Modules.User.Application.GettingUsers.Mappers;

/// <summary>
/// UserInfo to UserInfoDto mapper.
/// </summary>
internal static class UserInfoUserInfoDtoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="userInfo">Value to map.</param>
	/// <returns>Mapped object.</returns>
	public static UserInfoDto Map(UserInfo userInfo)
	{
		if (userInfo is null)
		{
			return new UserInfoDto();
		}

		return new UserInfoDto
		{
			Id = userInfo.Id,
			Avatar = userInfo.Attributes?.Avatar?.FirstOrDefault(),
			Email = userInfo.Email,
			CreatedTimestamp = userInfo.CreatedTimestamp,
			CreatedVia = userInfo.CreatedVia,
			FirstName = userInfo.FirstName,
			LastName = userInfo.LastName,
		};
	}
}