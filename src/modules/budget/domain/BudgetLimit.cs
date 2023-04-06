namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Budget limit value object.
/// </summary>
/// <param name="Value">Value.</param>
/// <param name="Currency">Currency.</param>
public record class BudgetLimit(float Value, string Currency);