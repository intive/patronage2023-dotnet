namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;

/// <summary>
/// Transaction sort descriptor.
/// </summary>
public class TransactionSortDescriptor
{
	/// <summary>
	/// Column.
	/// </summary>
	public int Column { get; set; }

	/// <summary>
	/// SortOrder.
	/// </summary>
	public bool SortAscending { get; set; }
}