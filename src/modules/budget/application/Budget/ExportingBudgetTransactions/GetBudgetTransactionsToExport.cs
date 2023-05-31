using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Modules.Budget.Application.Extensions;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgetTransactions;

/// <summary>
/// Record GetBudgetTransactionsToExport representing a query to retrieve budget transactions ready for export.
/// </summary>
public record GetBudgetTransactionsToExport() : IQuery<GetBudgetTransactionTransferList?>
{
	/// <summary>
	/// Budget id to export transactions from.
	/// </summary>
	public Guid BudgetId { get; set; }
}

/// <summary>
/// Handles the GetBudgetsToExport query by fetching the required budgets from the BudgetDbContext.
/// </summary>
public class GetBudgetTransactionsToExportQueryHandler : IQueryHandler<GetBudgetTransactionsToExport, GetBudgetTransactionTransferList?>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IExecutionContextAccessor contextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetTransactionsToExportQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	/// <param name="contextAccessor">IExecutionContextAccessor.</param>
	public GetBudgetTransactionsToExportQueryHandler(BudgetDbContext budgetDbContext, IExecutionContextAccessor contextAccessor)
	{
		this.budgetDbContext = budgetDbContext;
		this.contextAccessor = contextAccessor;
	}

	/// <summary>
	/// Handles the GetBudgetsToExport query.
	/// </summary>
	/// <param name="query">The GetBudgetsToExport query to be handled.</param>
	/// <param name="cancellationToken">A token that may be used to cancel the handle operation.</param>
	/// <returns>A GetBudgetTransferList containing the budgets to be exported, or null if no budgets are found.</returns>
	public async Task<GetBudgetTransactionTransferList?> Handle(GetBudgetTransactionsToExport query, CancellationToken cancellationToken)
	{
		var transactions = this.budgetDbContext.Transaction.For(new BudgetId(query.BudgetId))
			.Where(x => x.Status == Contracts.TransactionEnums.Status.Active);

		var transactionsInfos = await transactions.MapToGetBudgetTransactionTransferInfo().ToListAsync(cancellationToken: cancellationToken);

		return new GetBudgetTransactionTransferList { BudgetTransactionsList = transactionsInfos };
	}
}