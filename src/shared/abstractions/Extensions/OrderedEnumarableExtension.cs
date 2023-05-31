namespace Intive.Patronage2023.Shared.Abstractions.Extensions;

/// <summary>
/// OrderedEnumerableExtension class.
/// </summary>
public static class OrderedEnumarableExtension
{
	/// <summary>
	/// Pagination extension method.
	/// </summary>
	/// <typeparam name="T">Entity type.</typeparam>
	/// <param name="orderedEnumerable">Ordered enumerable.</param>
	/// <param name="pageableQuery">Paging criteria.</param>
	/// <returns>Paginated enumerable.</returns>
	public static IEnumerable<T> Paginate<T>(this IOrderedEnumerable<T> orderedEnumerable, IPageableQuery pageableQuery)
	{
		var result = orderedEnumerable.Skip(pageableQuery.PageSize * (pageableQuery.PageIndex - 1)).Take(pageableQuery.PageSize);
		return result;
	}
}