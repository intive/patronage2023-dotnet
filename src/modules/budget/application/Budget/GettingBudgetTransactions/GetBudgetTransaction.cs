using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Modules.Budget.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;

/// <summary>
/// Get Budgets query.
/// </summary>
public record GetBudgetTransaction() : IQuery<PagedList<BudgetTransactionInfo>>, IPageableQuery
{
	/// <summary>
	/// The amount of data to return.
	/// </summary>
	public int PageSize { get; set; }

	/// <summary>
	/// Requested page.
	/// </summary>
	public int PageIndex { get; set; }

	/// <summary>
	/// Budget Id.
	/// </summary>
	public BudgetId BudgetId { get; set; }
}

/// <summary>
/// Get Budgets handler.
/// </summary>
public class GetTransactionQueryHandler : IQueryHandler<GetBudgetTransaction, PagedList<BudgetTransactionInfo>>
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
	public async Task<PagedList<BudgetTransactionInfo>> Handle(GetBudgetTransaction query, CancellationToken cancellationToken)
	{
		var budgets = this.budgetDbContext.Transaction.AsQueryable();

		var mappedData = await budgets.Select(BudgetTransactionInfoMapper.Map).Where(x => x.BudgetId == query.BudgetId).Paginate(query).OrderBy(x => x.BudgetTransactionDate).ToListAsync();
		var result = new PagedList<BudgetTransactionInfo> { Items = mappedData };
		return result;
	}
}