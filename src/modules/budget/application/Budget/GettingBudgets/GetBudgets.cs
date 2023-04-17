using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Application.Extensions;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;

/// <summary>
/// Get budgets query.
/// </summary>
public record GetBudgets() : IQuery<PagedList<BudgetInfo>>, IPageableQuery, ITextSearchQuery, ISortableQuery
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
	/// Field to search budget by name.
	/// </summary>
	public string? Search { get; set; }

	/// <summary>
	/// List of criteria to sort budgets.
	/// </summary>
	public List<SortDescriptor>? SortDescriptors { get; set; }
}

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

		if (!string.IsNullOrEmpty(query.Search))
		{
			budgets = budgets.Where(x => x.Name.Contains(query.Search));
		}

		var mappedData = await budgets.Sort(query).Paginate(query).Select(BudgetAggregateBudgetInfoMapper.Map).ToListAsync();
		int totalItemsCount = await budgets.CountAsync();
		var result = new PagedList<BudgetInfo> { Items = mappedData, TotalCount = totalItemsCount };
		return result;
	}
}