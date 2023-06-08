namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;

/// <summary>
/// Represents detailed information about a budget transfer.
/// </summary>
public record GetBudgetTransactionTransferInfo()
{
	/// <summary>
	/// Transaction name.
	/// </summary>
	public string Name { get; init; } = null!;

	/// <summary>
	/// Transaction creator email.
	/// </summary>
	public string Email { get; init; } = null!;

	/// <summary>
	/// Transaction value.
	/// </summary>
	public string Value { get; init; } = null!;

	/// <summary>
	/// Transaction type.
	/// </summary>
	public string TransactionType { get; init; } = null!;

	/// <summary>
	/// Transaction category type.
	/// </summary>
	public string CategoryType { get; init; } = null!;

	/// <summary>
	/// Transaction date.
	/// </summary>
	public string Date { get; init; } = null!;

	/// <summary>
	/// Transaction status.
	/// </summary>
	public string Status { get; init; } = null!;
}