using Intive.Patronage2023.Modules.Budget.Application.Data;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactionAttachment;

/// <summary>
/// Query getting budget transaction attachment.
/// </summary>
/// <param name="TransactionId">Transaction Id.</param>
public record GetBudgetTransactionAttachment(TransactionId TransactionId) : IQuery<IFormFile>;

/// <summary>
/// Method that handles getting budget transaction attachment.
/// </summary>
public class HandleGetBudgetTransactionAttachment : IQueryHandler<GetBudgetTransactionAttachment, IFormFile>
{
	private readonly IBlobStorageService blobStorageService;
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleGetBudgetTransactionAttachment"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget Db Context.</param>
	/// <param name="blobStorageService">Blob storage service.</param>
	public HandleGetBudgetTransactionAttachment(IBlobStorageService blobStorageService, BudgetDbContext budgetDbContext)
	{
		this.blobStorageService = blobStorageService;
		this.budgetDbContext = budgetDbContext;
	}

	/// <inheritdoc />
	public Task<IFormFile> Handle(GetBudgetTransactionAttachment query, CancellationToken cancellationToken)
	{
		var budgetTransaction = this.budgetDbContext.Transaction.FirstOrDefaultAsync(t => t.Id == query.TransactionId);

		Uri url = budgetTransaction.Result?.AttachmentUrl ?? throw new ApplicationException("Could not find provided transaction!");

		var file = this.blobStorageService.GetFileFromUrlAsync(url);

		return file;
	}
}