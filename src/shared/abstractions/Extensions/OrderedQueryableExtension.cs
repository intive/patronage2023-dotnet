namespace Intive.Patronage2023.Shared.Abstractions.Extensions;

/// <summary>
/// OrderedQueryableExtension class.
/// </summary>
public static class OrderedQueryableExtension
{
	/// <summary>
	/// Pagination extension method.
	/// </summary>
	/// <typeparam name="T">Entity type.</typeparam>
	/// <param name="query">Query.</param>
	/// <param name="pageIndex">Page index.</param>
	/// <param name="pageSize">Page size.</param>
	/// <returns>Paginated query.</returns>
	public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageIndex, int pageSize)
	{
		var result = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
		return result;
	}
}