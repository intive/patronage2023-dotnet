using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;

/// <summary>
/// Get Budgets query.
/// </summary>
public record GetBudgets() : IQuery<PagedList<BudgetInfo>>;

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
		var budgets = await this.budgetDbContext.Budget.OrderBy(x => x.BudgetId).ToListAsync();
		var mappedData = budgets.Select(BudgetAggregateBudgetInfoMapper.Map).ToList();
		var result = new PagedList<BudgetInfo> { Items = mappedData };
		return result;
	}
}