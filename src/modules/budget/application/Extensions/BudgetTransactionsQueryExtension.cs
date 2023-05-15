using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Extensions;

/// <summary>
/// BudgetTransaction Query Extension class.
/// </summary>
internal static class BudgetTransactionsQueryExtension
{
	/// <summary>
	/// Specifies BudgetTransactions for which query is done.
	/// </summary>
	/// <param name="query">query.</param>
	/// <param name="id">id.</param>
	/// <returns>Transactions with budgetId passed in method.</returns>
	public static IQueryable<BudgetTransactionAggregate> For(this IQueryable<BudgetTransactionAggregate> query, BudgetId id)
	{
		return query.Where(x => x.BudgetId == id);
	}

	/// <summary>
	/// Checks if date of transactions is in specified date.
	/// </summary>
	/// <param name="query">query.</param>
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
}