using Intive.Patronage2023.Shared.Abstractions.Commands;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;

/// <summary>
/// Create Budget command.
/// </summary>
/// <param name="Name">Budget name.</param>
/// <param name="Limit">Budget limit.</param>
/// <param name="Currency">Currency.</param>
/// <param name="StarTime">StarTime.</param>
/// <param name="EndTime">EndTime.</param>
/// <param name="Description">Description.</param>
/// <param name="IconName">Budget icon identifier.</param>
public record CreateBudgetsToImport(string Name, string Limit, string Currency, string StarTime, string EndTime, string IconName, string Description) : ICommand;