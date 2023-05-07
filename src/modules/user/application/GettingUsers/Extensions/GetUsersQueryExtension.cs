using Intive.Patronage2023.Shared.Abstractions;

namespace Intive.Patronage2023.Modules.User.Application.GettingUsers.Extensions;

/// <summary>
/// Get users query extension class.
/// </summary>
internal static class GetUsersQueryExtension
{
	/// <summary>
	/// Sorting extension method.
	/// </summary>
	/// <param name="users">Query.</param>
	/// <param name="sortDescriptors">Sort criteria.</param>
	/// <returns>Sorted query.</returns>
	public static IOrderedEnumerable<UserInfo> Sort(this List<UserInfo> users, List<SortDescriptor> sortDescriptors)
	{
		if (sortDescriptors.Count() == 0)
		{
			return users.OrderBy(x => x.Id);
		}

		var result = Sort(users, sortDescriptors.First());

		foreach (var sortDescriptor in sortDescriptors.Skip(1))
		{
			result = Sort(users!, sortDescriptor!);
		}

		return result;
	}

	/// <summary>
	/// Sorting helper method.
	/// </summary>
	/// <param name="users">Query.</param>
	/// <param name="sortDescriptor">Sort criteria.</param>
	/// <returns>Sorted query.</returns>
	private static IOrderedEnumerable<UserInfo> Sort(List<UserInfo> users, SortDescriptor sortDescriptor)
	{
		switch (sortDescriptor.ColumnName.ToLower())
		{
			case "email": return sortDescriptor.SortAscending ? users.OrderBy(x => x.Email) : users.OrderByDescending(x => x.Email);
			case "firstname": return sortDescriptor.SortAscending ? users.OrderBy(x => x.FirstName) : users.OrderByDescending(x => x.FirstName);
			case "lastname": return sortDescriptor.SortAscending ? users.OrderBy(x => x.LastName) : users.OrderByDescending(x => x.LastName);
			case "createdtimestamp": return sortDescriptor.SortAscending ? users.OrderBy(x => x.CreatedTimestamp) : users.OrderByDescending(x => x.CreatedTimestamp);
			default: throw new NotSupportedException($"{sortDescriptor.ColumnName} is not supported yet");
		}
	}
}