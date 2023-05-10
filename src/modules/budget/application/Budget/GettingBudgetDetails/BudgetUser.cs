namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;

/// <summary>
/// Contains informations about budget user id and his avatar.
/// </summary>
public record BudgetUser(
	Guid Id,
	string? Avatar);