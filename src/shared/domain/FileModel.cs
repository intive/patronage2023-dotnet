namespace Intive.Patronage2023.Shared.Domain;

/// <summary>
/// Attachment file model.
/// </summary>
public class FileModel
{
	/// <summary>
	/// File name.
	/// </summary>
	public string? FileName { get; set; }

	/// <summary>
	/// File content.
	/// </summary>
	public Stream? Content { get; set; }
}