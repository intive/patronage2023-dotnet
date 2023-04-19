using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingTransaction;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingTransaction;

/// <summary>
/// Get Budgets query.
/// </summary>
public record GetTransaction(BudgetId BudgetId) : IQuery<PagedList<TransactionInfo>>;

/// <summary>
/// Get Budgets handler.
/// </summary>
public class GetTransactionQueryHandler : IQueryHandler<GetTransaction, PagedList<TransactionInfo>>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetTransactionQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	public GetTransactionQueryHandler(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// GetBudgets query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>Paged list of Budgets.</returns>
	public async Task<PagedList<TransactionInfo>> Handle(GetTransaction query, CancellationToken cancellationToken)
	{
		var transcations = await this.budgetDbContext.Transaction.Where(x => x.BudgetId == query.BudgetId).ToListAsync();
		var mappedData = transcations.Select(TransactionAggregateInfoMapper.Map).ToList();
		var result = new PagedList<TransactionInfo> { Items = mappedData };
		return result;
	}
}