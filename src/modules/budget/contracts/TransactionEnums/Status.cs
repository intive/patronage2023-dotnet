namespace Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

/// <summary>
/// Enum of status Budget and its related Transactions.
/// </summary>
public enum Status
{
	/// <summary>
	/// Default value of created Budget or Transaction.
	/// </summary>
	Active = 1,

	/// <summary>
	/// Status of soft deleted budget or transaction.
	/// </summary>
	Deleted = 2,

	/// <summary>
	/// Status of cancelled budget or transaction.
	/// </summary>
	Cancelled = 3,

	/// <summary>
	/// Status of transaction that will be reccuring in certain period of time.
	/// </summary>
	Reccuring = 4,

	/// <summary>
	/// Status of transaction.
	/// </summary>
	Due = 5,
}