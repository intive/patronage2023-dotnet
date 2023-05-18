using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.Data;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.AddingBudgetTransactionAttachment;

/// <summary>
/// Command adding budget transaction attachment.
/// </summary>
/// <param name="File">File.</param>
public record AddingBudgetTransactionAttachment(IFormFile File, TransactionId TransactionId) : ICommand;

/// <summary>
/// Method that handles adding attachment to budget transaction.
/// </summary>
public class HandleAddingBudgetTransactionAttachment : ICommandHandler<AddingBudgetTransactionAttachment>
{
	private readonly IBlobStorageService blobStorageService;
	private readonly AddingBudgetTransactionAttachmentValidator attachmentValidator;
	private readonly BudgetTransactionRepository budgetTransactionRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleAddingBudgetTransactionAttachment"/> class.
	/// Constructor.
	/// </summary>
	/// <param name="blobStorageService">Blob storage service.</param>
	/// <param name="attachmentValidator">Attachment validator.</param>
	/// <param name="budgetTransactionRepository">Budget transaction repository.</param>
	public HandleAddingBudgetTransactionAttachment(IBlobStorageService blobStorageService, AddingBudgetTransactionAttachmentValidator attachmentValidator, BudgetTransactionRepository budgetTransactionRepository)
	{
		this.blobStorageService = blobStorageService;
		this.attachmentValidator = attachmentValidator;
		this.budgetTransactionRepository = budgetTransactionRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(AddingBudgetTransactionAttachment command, CancellationToken cancellationToken)
	{
		var file = command.File;

		var validationResult = this.attachmentValidator.Validate(file);
		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}

		var attachmentFile = new BudgetTransactionAttachmentModel()
		{
			FileName = file.Name,
			Content = file.OpenReadStream(),
		};

		string fileUrl = this.blobStorageService.UploadFileAsync("user-name", attachmentFile.FileName, attachmentFile.Content).ToString()!;

		BudgetTransactionAggregate? transaction = await this.budgetTransactionRepository.GetById(command.TransactionId);
		transaction?.AddAttachment(fileUrl);
		await this.budgetTransactionRepository.Persist(transaction ?? throw new InvalidOperationException());
	}
}