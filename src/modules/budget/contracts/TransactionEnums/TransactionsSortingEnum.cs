namespace Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

/// <summary>
/// Enum of transaction sorting columns.
/// </summary>
public enum TransactionsSortingEnum
{
	/// <summary>
	/// Transaction name.
	/// </summary>
	Name = 0,

	/// <summary>
	/// Transaction category.
	/// </summary>
	CategoryType = 1,

	/// <summary>
	/// Transaction status.
	/// </summary>
	Status = 2,

	/// <summary>
	/// Transaction value.
	/// </summary>
	Value = 3,

	/// <summary>
	/// Transaction date.
	/// </summary>
	BudgetTransactionDate = 4,

	/// <summary>
	/// Transaction creator email.
	/// </summary>
	Email = 5,
}