using CsvHelper.Configuration.Attributes;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.AddingBudgetTransactionAttachment;

/// <summary>
/// Adding attachment file to transaction validator class.
/// </summary>
public class AddingBudgetTransactionAttachmentValidator : AbstractValidator<IFormFile>
{
	private readonly long maxFileSize = 5000000;
	private readonly string[] allowedFileExtensions = new string[] { "PDF", "JPG", "BMP", "PNG" };

	/// <summary>
	/// Initializes a new instance of the <see cref="AddingBudgetTransactionAttachmentValidator"/> class.
	/// Attachment file validator.
	/// </summary>
	/// <param name="maxFileSize">Maximum file size parameter.</param>
	/// <param name="allowedFileExtensions">Allowed file extensions parameter. </param>
	public AddingBudgetTransactionAttachmentValidator(long maxFileSize, string[] allowedFileExtensions)
	{
		this.maxFileSize = maxFileSize;
		this.allowedFileExtensions = allowedFileExtensions;

		this.RuleFor(file => file.Length)
			.LessThanOrEqualTo(maxFileSize)
			.WithMessage($"File size must be less than {maxFileSize} bytes.");
		this.RuleFor(file => Path.GetExtension(file.FileName))
			.Must(extention => allowedFileExtensions.Contains(extention.ToLower()))
			.WithMessage($"File extension must fall into: {string.Join(", ", this.allowedFileExtensions)}");
	}
}