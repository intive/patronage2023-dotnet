using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Modules.Budget.Application.Extensions;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgetTransactions;

/// <summary>
/// Record GetBudgetTransactionsToExport representing a query to retrieve budget transactions ready for export.
/// </summary>
public record GetBudgetTransactionsToExport() : IQuery<GetTransferList<GetBudgetTransactionTransferInfo>?>
{
	/// <summary>
	/// Budget id to export transactions from.
	/// </summary>
	public BudgetId BudgetId { get; set; }
}

/// <summary>
/// Handles the GetBudgetsToExport query by fetching the required budgets from the BudgetDbContext.
/// </summary>
public class GetBudgetTransactionsToExportQueryHandler : IQueryHandler<GetBudgetTransactionsToExport, GetTransferList<GetBudgetTransactionTransferInfo>?>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetTransactionsToExportQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	/// <param name="contextAccessor">IExecutionContextAccessor.</param>
	public GetBudgetTransactionsToExportQueryHandler(BudgetDbContext budgetDbContext, IExecutionContextAccessor contextAccessor)
	{
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// Handles the GetBudgetsToExport query.
	/// </summary>
	/// <param name="query">The GetBudgetsToExport query to be handled.</param>
	/// <param name="cancellationToken">A token that may be used to cancel the handle operation.</param>
	/// <returns>A GetBudgetTransferList containing the budgets to be exported, or null if no budgets are found.</returns>
	public async Task<GetTransferList<GetBudgetTransactionTransferInfo>?> Handle(GetBudgetTransactionsToExport query, CancellationToken cancellationToken)
	{
		var transactions = this.budgetDbContext.Transaction.For(query.BudgetId)
			.Where(x => x.Status == Status.Active || x.Status == Status.Cancelled);

		var transactionsInfos = await transactions.MapToGetBudgetTransactionTransferInfo().ToListAsync(cancellationToken: cancellationToken);

		return new GetTransferList<GetBudgetTransactionTransferInfo> { CorrectList = transactionsInfos };
	}
}