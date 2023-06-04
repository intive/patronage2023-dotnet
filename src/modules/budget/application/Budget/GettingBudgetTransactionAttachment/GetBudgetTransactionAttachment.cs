using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactionAttachment;

/// <summary>
/// Query getting budget transaction attachment.
/// </summary>
/// <param name="TransactionId">Transaction Id.</param>
public record GetBudgetTransactionAttachment(TransactionId TransactionId) : IQuery<Uri>;

/// <summary>
/// Method that handles getting budget transaction attachment.
/// </summary>
public class HandleGetBudgetTransactionAttachment : IQueryHandler<GetBudgetTransactionAttachment, Uri>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleGetBudgetTransactionAttachment"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget Db Context.</param>
	public HandleGetBudgetTransactionAttachment(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
	}

	/// <inheritdoc />
	public Task<Uri> Handle(GetBudgetTransactionAttachment query, CancellationToken cancellationToken)
	{
		var budgetTransaction = this.budgetDbContext.Transaction.FirstOrDefaultAsync(t => t.Id == query.TransactionId);

		Uri url = budgetTransaction.Result?.AttachmentUrl ?? throw new ApplicationException("Could not find provided transaction!");

		return Task.FromResult(url);
	}
}