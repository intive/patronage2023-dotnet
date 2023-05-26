namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;

/// <summary>
/// Class ImportResult representing the result of an import operation, containing any encountered error messages and the URI used for import.
/// </summary>
public record ImportResult
{
	/// <summary>
	/// List of error messages encountered during the import operation. If no errors occurred, this will be null or empty.
	/// </summary>
	public List<string>? ErrorsList { get; init; }

	/// <summary>
	/// URI that was used for the import operation. This could represent a file location in Azure Blob Storage.
	/// </summary>
	public string? Uri { get; init; }
}