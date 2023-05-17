using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.EditingBudget;

/// <summary>
/// Edit Budget command.
/// </summary>
/// <param name="Name">Budget name.</param>
/// <param name="Period">Budget time span.</param>
/// <param name="Description">Description.</param>
/// <param name="IconName">Budget icon identifier.</param>
public record EditBudgetDetails(
	string Name,
	Period Period,
	string Description,
	string IconName);