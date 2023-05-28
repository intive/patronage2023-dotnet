using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.AddingBudgetTransactionAttachment;

/// <summary>
/// Adding attachment file to transaction validator class.
/// </summary>
public class AddBudgetTransactionAttachmentValidator : AbstractValidator<AddBudgetTransactionAttachment>
{
	private readonly long maxFileSize = 5000000;
	private readonly string[] allowedFileExtensions = { ".pdf", ".jpg", ".bmp", ".png" };

	/// <summary>
	/// Initializes a new instance of the <see cref="AddBudgetTransactionAttachmentValidator"/> class.
	/// Attachment file validator.
	/// </summary>
	public AddBudgetTransactionAttachmentValidator()
	{
		this.RuleFor(command => command.File)
			.NotNull().WithMessage("File cannot be null.")
			.Must(this.HaveValidExtension).WithMessage($"File extension must be one of the following: {string.Join(", ", this.allowedFileExtensions)}.")
			.Must(this.HaveValidSize).WithMessage($"File size must be less than {this.maxFileSize} bytes.");
	}

	private bool HaveValidExtension(IFormFile file)
	{
		string extension = Path.GetExtension(file.FileName);
		return this.allowedFileExtensions.Contains(extension.ToLower());
	}

	private bool HaveValidSize(IFormFile file)
	{
		return file.Length <= this.maxFileSize;
	}
}