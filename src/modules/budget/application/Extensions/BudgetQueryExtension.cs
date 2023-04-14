using System.Linq.Expressions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Shared.Abstractions;

namespace Intive.Patronage2023.Modules.Budget.Application.Extensions;

/// <summary>
/// Budget query extension class.
/// </summary>
internal static class BudgetQueryExtension
{
	/// <summary>
	/// Sorting extension method.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="sortableQuery">Sorting criteria.</param>
	/// <returns>Sorted query.</returns>
	public static IQueryable<BudgetInfo> Sort(this IQueryable<BudgetInfo> query, ISortableQuery sortableQuery)
	{
		if (sortableQuery is null || !sortableQuery.SortDescriptors.Any())
		{
			return query;
		}

		foreach (var sortDescriptor in sortableQuery.SortDescriptors)
		{
			query = Sort(query, sortDescriptor);
		}

		return query;
	}

	/// <summary>
	/// Projection method.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function represented by selector.</typeparam>
	/// <param name="source">A sequence of values to project.</param>
	/// <param name="selector">A projection function to apply to each element.</param>
	/// <returns>An IQueryable whose elements are the result of invoking a projection function on each element of source.</returns>
	public static IQueryable<TResult> ProjectBy<TSource, TResult>(this IQueryable<TSource> source, Func<TSource, TResult> selector)
	{
		var expression = Expression.Lambda<Func<TSource, TResult>>(Expression.Call(selector.Method));
		return source.Select(expression);
	}

	private static IQueryable<BudgetInfo> Sort(IQueryable<BudgetInfo> query, SortDescriptor descriptor)
	{
		switch (descriptor.ColumnName.ToLower())
		{
			case "id": return descriptor.SortAscending ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
			case "name": return descriptor.SortAscending ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
			case "createdon": return descriptor.SortAscending ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
			default: throw new NotSupportedException($"{descriptor.ColumnName} is not supported yet");
		}
	}
}