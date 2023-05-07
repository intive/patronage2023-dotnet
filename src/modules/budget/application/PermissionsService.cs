using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Application.Extensions;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application;

/// <summary>
/// Class PermissionsService.
/// </summary>
public class PermissionsService
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="PermissionsService"/> class.
	/// </summary>
	/// <param name="contextAccessor">IExecutionContextAccessor.</param>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	public PermissionsService(IExecutionContextAccessor contextAccessor, BudgetDbContext budgetDbContext)
	{
		this.contextAccessor = contextAccessor;
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// GetBudgets query method.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>Paged list of Budgets.</returns>
	public async Task<PagedList<BudgetInfo>> GetBudgets(GetBudgets query, CancellationToken cancellationToken)
	{
		bool isAdmin = this.contextAccessor.IsUserAdmin();
		var budgets = this.budgetDbContext.Budget.AsQueryable();

		if (!isAdmin)
		{
			var userId = this.contextAccessor.GetUserId();
			var userBudgets = this.budgetDbContext.UserBudget.AsEnumerable().Where(x => x.UserId.Value == userId).Select(y => y.BudgetId).ToList();
			budgets = budgets.Where(x => userBudgets.Contains(x.Id)).AsQueryable();
		}

		if (!string.IsNullOrEmpty(query.Search))
		{
			budgets = budgets.Where(x => x.Name.Contains(query.Search));
		}

		var mappedData = await budgets.Select(BudgetAggregateBudgetInfoMapper.Map).Sort(query).Paginate(query).ToListAsync(cancellationToken: cancellationToken);
		int totalItemsCount = await budgets.CountAsync(cancellationToken: cancellationToken);
		var result = new PagedList<BudgetInfo> { Items = mappedData, TotalCount = totalItemsCount };
		return result;
	}

	/// <summary>
	/// GetBudgetDetails query method.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>BudgetDetailsInfo or null.</returns>
	public async Task<BudgetDetailsInfo?> GetBudgetDetails(GetBudgetDetails query, CancellationToken cancellationToken)
	{
		bool isAdmin = this.contextAccessor.IsUserAdmin();
		var budgetId = new BudgetId(query.Id);
		var userId = this.contextAccessor.GetUserId();
		bool isPermissions = this.budgetDbContext.UserBudget.AsEnumerable().Any(x => x.UserId.Value == userId && x.BudgetId == budgetId);

		if (isAdmin || isPermissions)
		{
			var budget = await this.budgetDbContext.Budget.FindAsync(new object?[] { budgetId }, cancellationToken: cancellationToken);

			return budget is null ? null : BudgetAggregateBudgetDetailsInfoMapper.Map(budget);
		}

		return null;
	}
}