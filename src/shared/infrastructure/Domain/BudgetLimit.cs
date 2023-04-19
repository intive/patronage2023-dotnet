namespace Intive.Patronage2023.Shared.Infrastructure.Domain;

/// <summary>
/// Budget limit value object.
/// </summary>
/// <param name="Value">Value.</param>
/// <param name="Currency">Currency.</param>
public record class BudgetLimit(decimal Value, Currency Currency);