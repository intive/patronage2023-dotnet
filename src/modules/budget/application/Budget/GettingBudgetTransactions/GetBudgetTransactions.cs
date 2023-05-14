using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;

/// <summary>
/// Get Budget's Transactions query.
/// </summary>
public record GetBudgetTransactions : IQuery<PagedList<BudgetTransactionInfo>>, IPageableQuery, ITextSearchQuery
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
	public BudgetId BudgetId { get; init; }

	/// <summary>
	/// Search text.
	/// </summary>
	public string? Search { get; set; }
}

/// <summary>
/// Get Budget's Transaction handler.
/// </summary>
public class GetTransactionsQueryHandler : IQueryHandler<GetBudgetTransactions, PagedList<BudgetTransactionInfo>>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetTransactionsQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	public GetTransactionsQueryHandler(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// GetBudgets query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>Paged list of Budgets.</returns>
	public async Task<PagedList<BudgetTransactionInfo>> Handle(GetBudgetTransactions query, CancellationToken cancellationToken)
	{
		var budgets = this.budgetDbContext.Transaction.AsQueryable();

		if (!string.IsNullOrEmpty(query.Search))
		{
			budgets = budgets.Where(x => x.Name.Contains(query.Search));
		}

		int totalItemsCount = await budgets
			.Where(x => x.BudgetId == query.BudgetId)
			.CountAsync(cancellationToken: cancellationToken);
		var mappedData = await budgets.Select(BudgetTransactionInfoMapper.Map)
			.Where(x => x.BudgetId == query.BudgetId)
			.Paginate(query)
			.OrderBy(x => x.BudgetTransactionDate)
			.ToListAsync(cancellationToken: cancellationToken);
		var result = new PagedList<BudgetTransactionInfo> { Items = mappedData, TotalCount = totalItemsCount };
		return result;
	}
}