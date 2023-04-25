namespace Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
/// <summary>
/// Record That represents Transaction Id.
/// </summary>
/// <param name="Value">Id of Transaction.</param>
public record struct TransactionId(Guid Value);