using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Domain;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.AddingBudgetTransactionAttachment;

/// <summary>
/// Command adding budget transaction attachment.
/// </summary>
/// <param name="File">File.</param>
/// <param name="TransactionId">Budget transaction Id.</param>
/// <param name="BudgetId">Budget Id.</param>
public record AddBudgetTransactionAttachment(IFormFile File, TransactionId TransactionId, BudgetId BudgetId) : ICommand;

/// <summary>
/// Method that handles adding attachment to budget transaction.
/// </summary>
public class HandleAddBudgetTransactionAttachment : ICommandHandler<AddBudgetTransactionAttachment>
{
	private readonly IBlobStorageService blobStorageService;
	private readonly IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleAddBudgetTransactionAttachment"/> class.
	/// </summary>
	/// <param name="blobStorageService">Blob storage service.</param>
	/// <param name="budgetTransactionRepository">Budget transaction repository.</param>
	public HandleAddBudgetTransactionAttachment(IBlobStorageService blobStorageService, IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository)
	{
		this.blobStorageService = blobStorageService;
		this.budgetTransactionRepository = budgetTransactionRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(AddBudgetTransactionAttachment command, CancellationToken cancellationToken)
	{
		var file = command.File;

		var attachmentFile = new FileModel()
		{
			FileName = command.TransactionId.Value.ToString(),
			Content = file.OpenReadStream(),
		};

		var transaction = await this.budgetTransactionRepository.GetById(command.TransactionId);

		string blobName = await this.blobStorageService.UploadToBlobStorage(attachmentFile.Content, attachmentFile.FileName);

		transaction!.AddAttachment(blobName);

		await this.budgetTransactionRepository.Persist(transaction);
	}
}