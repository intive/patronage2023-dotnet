namespace Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

/// <summary>
/// Enum of transaction sorting columns.
/// </summary>
public enum TransactionsSortingEnum
{
	/// <summary>
	/// Transaction name.
	/// </summary>
	Name = 1,

	/// <summary>
	/// Transaction category.
	/// </summary>
	CategoryType = 2,

	/// <summary>
	/// Transaction status.
	/// </summary>
	Status = 3,

	/// <summary>
	/// Transaction value.
	/// </summary>
	Value = 4,

	/// <summary>
	/// Budget transaction date.
	/// </summary>
	BudgetTransactionDate = 5,

	/// <summary>
	/// Budget transaction creator username.
	/// </summary>
	Username = 6,
}