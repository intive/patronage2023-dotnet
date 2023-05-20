namespace Intive.Patronage2023.Modules.Budget.Application.Data;

/// <summary>
/// Class ImportResult.
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