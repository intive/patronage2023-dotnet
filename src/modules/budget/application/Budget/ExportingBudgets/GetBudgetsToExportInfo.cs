namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

///
/// <summary>
/// Create Budget command.
/// </summary>
/// <param name="Name">Budget name.</param>
/// <param name="CreatedOn">CreatedOn.</param>
/// <param name="IconName">Budget icon identifier.</param>
/// <param name="Description">Description.</param>
/// <param name="Currency">Currency.</param>
/// <param name="Value">Budget value.</param>
/// <param name="StartDate">StartDate.</param>
/// <param name="EndDate">EndDate.</param>

public record GetBudgetsToExportInfo(string Name, string CreatedOn, string IconName, string Description, string Currency, string Value, string StartDate, string EndDate);