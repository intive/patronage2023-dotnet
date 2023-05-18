namespace Intive.Patronage2023.Modules.Budget.Application.Data;

/// <summary>
/// ImportResult.
/// </summary>
public record ImportResult
{
	/// <summary>
	/// ErrorsList.
	/// </summary>
	public List<string>? ErrorsList { get; init; }

	/// <summary>
	/// Uri.
	/// </summary>
	public string? Uri { get; init; }
}