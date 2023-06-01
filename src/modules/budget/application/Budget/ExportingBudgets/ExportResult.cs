namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
/// <summary>
/// Class ExportResult representing the result of an export operation, containing the URI used for export.
/// </summary>
public record ExportResult
{
	/// <summary>
	/// URI that was used for the export operation. This could represent a file location in Azure Blob Storage.
	/// </summary>
	public string? Uri { get; init; }
}