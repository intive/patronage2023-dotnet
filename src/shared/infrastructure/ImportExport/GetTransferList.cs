namespace Intive.Patronage2023.Shared.Infrastructure.ImportExport;

/// <summary>
/// Recotd to keep lists of correct and incorrect objects of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Transfer record type.</typeparam>
public record GetTransferList<T>()
{
	/// <summary>
	/// Contains a list of correct transfer values details.
	/// </summary>
	public List<T> CorrectList { get; init; } = default!;

	/// <summary>
	/// Contains a list of transfer details with errors.
	/// </summary>
	public List<T> ErrorsList { get; init; } = default!;
}