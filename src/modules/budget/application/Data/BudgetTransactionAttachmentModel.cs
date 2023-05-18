namespace Intive.Patronage2023.Modules.Budget.Application.Data;

/// <summary>
/// Attachment file model.
/// </summary>
public class BudgetTransactionAttachmentModel
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