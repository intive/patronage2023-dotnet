using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;

/// <summary>
/// Get budgets query.
/// </summary>
/// <param name="Search">Field to search budget by name.</param>
/// <param name="SortAscending">Bool to sort budgets ascending/descending by name.</param>
/// <param name="PageSize">The amount of data to return.</param>
/// <param name="PageIndex">Requested page.</param>
public record GetBudgets(string Search, int PageSize, int PageIndex, bool SortAscending = true) : IQuery<PagedList<BudgetInfo>>;

/// <summary>
/// Get Budgets handler.
/// </summary>
public class GetBudgetQueryHandler : IQueryHandler<GetBudgets, PagedList<BudgetInfo>>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	public GetBudgetQueryHandler(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// GetBudgets query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>Paged list of Budgets.</returns>
	public async Task<PagedList<BudgetInfo>> Handle(GetBudgets query, CancellationToken cancellationToken)
	{
		var budgets = this.budgetDbContext.Budget.AsQueryable();

		if (string.IsNullOrEmpty(query.Search))
		{
			budgets = budgets.Where(x => x.Name.StartsWith(query.Search, StringComparison.OrdinalIgnoreCase));
		}

		if (!query.SortAscending)
		{
			budgets = budgets.OrderByDescending(x => x.Name);
		}
		else
		{
			budgets = budgets.OrderBy(x => x.Name);
		}

		var results = await budgets.Skip(query.PageIndex * query.PageSize).Take(query.PageSize).ToListAsync();
		var mappedData = results.Select(BudgetAggregateBudgetInfoMapper.Map).ToList();
		var result = new PagedList<BudgetInfo> { Items = mappedData };
		return result;
	}
}