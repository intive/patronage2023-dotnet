using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactionAttachment;

/// <summary>
/// Query getting budget transaction attachment.
/// </summary>
/// <param name="TransactionId">Transaction Id to get attachment for.</param>
/// <param name="BudgetId">Transaction's budget id.</param>
public record GetBudgetTransactionAttachment(TransactionId TransactionId, BudgetId BudgetId) : IQuery<Uri>;

/// <summary>
/// Method that handles getting budget transaction attachment.
/// </summary>
public class HandleGetBudgetTransactionAttachment : IQueryHandler<GetBudgetTransactionAttachment, Uri>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IBlobStorageService blobStorageService;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleGetBudgetTransactionAttachment"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget Db Context.</param>
	/// /// <param name="blobStorageService">Blob storage service.</param>
	public HandleGetBudgetTransactionAttachment(BudgetDbContext budgetDbContext, IBlobStorageService blobStorageService)
	{
		this.budgetDbContext = budgetDbContext;
		this.blobStorageService = blobStorageService;
	}

	/// <inheritdoc />
	public async Task<Uri> Handle(GetBudgetTransactionAttachment query, CancellationToken cancellationToken)
	{
		var budgetTransaction = await this.budgetDbContext.Transaction.FirstOrDefaultAsync(t => t.Id == query.TransactionId, cancellationToken: cancellationToken);

		string fileUrlString = await this.blobStorageService.GenerateLinkToDownload(budgetTransaction!.AttachmentUrl!);
		var attachmentUrl = new Uri(fileUrlString);

		return attachmentUrl;
	}
}