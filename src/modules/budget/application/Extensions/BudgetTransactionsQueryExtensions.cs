using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;

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
	public static IQueryable<BudgetTransactionAggregate> WithCategoryTypes(this IQueryable<BudgetTransactionAggregate> query, string[]? categoryTypes)
	{
		return categoryTypes is null || !categoryTypes.Any() ? query : query.Where(x => categoryTypes.Contains(x.CategoryType));
	}
}