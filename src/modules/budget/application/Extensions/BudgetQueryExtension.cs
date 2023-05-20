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

		var orderedQuery = SortOrderBy(query, sortableQuery.SortDescriptors.First());

		for (int i = 1; i < sortableQuery.SortDescriptors.Count; i++)
		{
			orderedQuery = SortThenBy(orderedQuery, sortableQuery.SortDescriptors[i]);
		}

		return orderedQuery;
	}

	/// <summary>
	/// Sorting order by helper method.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="descriptor">Sort criteria.</param>
	/// <returns>Sorted query.</returns>
	private static IOrderedQueryable<BudgetInfo> SortOrderBy(IQueryable<BudgetInfo> query, SortDescriptor descriptor)
	{
		switch (descriptor.ColumnName.ToLower())
		{
			case "id": return descriptor.SortAscending ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
			case "name": return descriptor.SortAscending ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
			case "createdon": return descriptor.SortAscending ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
			case "isfavourite": return descriptor.SortAscending ? query.OrderBy(x => x.IsFavourite) : query.OrderByDescending(x => x.IsFavourite);
			default: throw new NotSupportedException($"{descriptor.ColumnName} is not supported yet");
		}
	}

	/// <summary>
	/// Sorting helper method.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="descriptor">Sort criteria.</param>
	/// <returns>Sorted query.</returns>
	private static IOrderedQueryable<BudgetInfo> SortThenBy(IOrderedQueryable<BudgetInfo> query, SortDescriptor descriptor)
	{
		switch (descriptor.ColumnName.ToLower())
		{
			case "id": return descriptor.SortAscending ? query.ThenBy(x => x.Id) : query.ThenByDescending(x => x.Id);
			case "name": return descriptor.SortAscending ? query.ThenBy(x => x.Name) : query.ThenByDescending(x => x.Name);
			case "createdon": return descriptor.SortAscending ? query.ThenBy(x => x.CreatedOn) : query.ThenByDescending(x => x.CreatedOn);
			case "isfavourite": return descriptor.SortAscending ? query.ThenBy(x => x.IsFavourite) : query.ThenByDescending(x => x.IsFavourite);
			default: throw new NotSupportedException($"{descriptor.ColumnName} is not supported yet");
		}
	}
}