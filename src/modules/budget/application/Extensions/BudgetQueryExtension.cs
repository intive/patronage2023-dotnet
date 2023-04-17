using Intive.Patronage2023.Modules.Budget.Domain;
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
	public static IQueryable<BudgetAggregate> Sort(this IQueryable<BudgetAggregate> query, ISortableQuery sortableQuery)
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

	private static IQueryable<BudgetAggregate> Sort(IQueryable<BudgetAggregate> query, SortDescriptor descriptor)
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