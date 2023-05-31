using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Microsoft.IdentityModel.Tokens;

namespace Intive.Patronage2023.Modules.Budget.Application.Extensions;

/// <summary>
/// BudgetTransaction Query Extensions class.
/// </summary>
internal static class BudgetTransactionsQueryExtensions
{
	/// <summary>
	/// Specifies BudgetTransactions for which query is done.
	/// </summary>
	/// <param name="query">Query to filter.</param>
	/// <param name="id">id.</param>
	/// <returns>Transactions with budgetId passed in method.</returns>
	public static IQueryable<BudgetTransactionAggregate> For(this IQueryable<BudgetTransactionAggregate> query, BudgetId id)
	{
		return query.Where(x => x.BudgetId == id);
	}

	/// <summary>
	/// Checks if date of transactions is in between specified date.
	/// </summary>
	/// <param name="query">Query to filter.</param>
	/// <param name="startDate">Start date.</param>
	/// <param name="endDate">End date.</param>
	/// <returns>Transactions within certain period of time. </returns>
	public static IQueryable<BudgetTransactionAggregate> Within(this IQueryable<BudgetTransactionAggregate> query, DateTime startDate, DateTime endDate)
	{
		return query.Where(x => x.BudgetTransactionDate >= startDate && x.BudgetTransactionDate <= endDate);
	}

	/// <summary>
	/// Extension method which filters transactions with status set to cancelled.
	/// </summary>
	/// <param name="query">Query to filter.</param>
	/// <returns>Transactions which have different status than Cancelled.</returns>
	public static IQueryable<BudgetTransactionAggregate> NotCancelled(this IQueryable<BudgetTransactionAggregate> query)
	{
		return query.Where(x => x.Status != Status.Cancelled);
	}

	/// <summary>
	/// Extension method which filters transactions by given type.
	/// </summary>
	/// <param name="query">Query to filter.</param>
	/// <param name="type">Type to filter.</param>
	/// <returns>Transactions which have given type.</returns>
	public static IQueryable<BudgetTransactionAggregate> WithType(this IQueryable<BudgetTransactionAggregate> query, TransactionType? type)
	{
		return type is null ? query : query.Where(x => x.TransactionType == type);
	}

	/// <summary>
	/// Extension method which filters transactions by given category types.
	/// </summary>
	/// <param name="query">Query to filter.</param>
	/// <param name="categoryTypes">Category types to filter.</param>
	/// <returns>Transactions which have one of given types.</returns>
	public static IQueryable<BudgetTransactionAggregate> WithCategoryTypes(this IQueryable<BudgetTransactionAggregate> query, CategoryType[]? categoryTypes)
	{
		return categoryTypes is null || !categoryTypes.Any() ? query : query.Where(x => categoryTypes.Contains(x.CategoryType));
	}

	/// <summary>
	/// Sorting extension method.
	/// </summary>
	/// <param name="budgetTransactions">Query.</param>
	/// <param name="sortDescriptors">Sort criteria.</param>
	/// <returns>Sorted query.</returns>
	public static IOrderedEnumerable<BudgetTransactionAggregate> Sort(this IQueryable<BudgetTransactionAggregate> budgetTransactions, List<TransactionSortDescriptor> sortDescriptors)
	{
		var budgetTransactionsOrdered = budgetTransactions.AsEnumerable().OrderBy(t => 1);
		if (sortDescriptors.IsNullOrEmpty())
		{
			return budgetTransactionsOrdered;
		}

		var mapping = new Dictionary<int, Func<BudgetTransactionAggregate, object>>()
		{
			{ (int)TransactionsSortingEnum.Name, t => t.Name },
			{ (int)TransactionsSortingEnum.CategoryType, t => t.CategoryType },
			{ (int)TransactionsSortingEnum.Status, t => t.Status },
			{ (int)TransactionsSortingEnum.Value, t => t.Value },
			{ (int)TransactionsSortingEnum.BudgetTransactionDate, t => t.BudgetTransactionDate },
			{ (int)TransactionsSortingEnum.Username, t => t.Username },
		};
		budgetTransactionsOrdered = sortDescriptors[0].SortAscending ? budgetTransactionsOrdered.OrderBy(mapping[sortDescriptors[0].Column]) : budgetTransactionsOrdered.OrderByDescending(mapping[sortDescriptors[0].Column]);

		foreach (var sortDescriptor in sortDescriptors.Skip(1))
		{
			budgetTransactionsOrdered = sortDescriptor.SortAscending ? budgetTransactionsOrdered.ThenBy(mapping[sortDescriptor.Column]) : budgetTransactionsOrdered.ThenByDescending(mapping[sortDescriptor.Column]);
		}

		return budgetTransactionsOrdered;
	}
}