using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Application.Extensions;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
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
	public List<SortDescriptor> SortDescriptors { get; set; } = null!;
}

/// <summary>
/// Get Budgets handler.
/// </summary>
public class GetBudgetsQueryHandler : IQueryHandler<GetBudgets, PagedList<BudgetInfo>>
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetsQueryHandler"/> class.
	/// </summary>
	/// <param name="contextAccessor">IExecutionContextAccessor.</param>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	public GetBudgetsQueryHandler(IExecutionContextAccessor contextAccessor, BudgetDbContext budgetDbContext)
	{
		this.contextAccessor = contextAccessor;
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
		bool isAdmin = this.contextAccessor.IsAdmin();
		var budgets = this.budgetDbContext.Budget.AsQueryable();
		var userId = new UserId(this.contextAccessor.GetUserId()!.Value);
		var userBudgetsFavourite = await this.budgetDbContext.UserBudget.Where(x => x.UserId == userId && x.IsFavourite).Select(x => x.BudgetId).ToListAsync();

		if (!isAdmin)
		{
			var userBudgets = this.budgetDbContext.UserBudget.Where(x => x.UserId == userId).Select(y => y.BudgetId);
			budgets = budgets.Where(x => userBudgets.Contains(x.Id)).AsQueryable();
		}

		if (!string.IsNullOrEmpty(query.Search))
		{
			budgets = budgets.Where(x => x.Name.Contains(query.Search));
		}

		var sortByFavourite = new SortDescriptor
		{
			ColumnName = nameof(UserBudgetAggregate.IsFavourite),
			SortAscending = false,
		};

		query.SortDescriptors.Insert(0, sortByFavourite);

		var items = await budgets
			.MapToBudgetInfo(userBudgetsFavourite)
			.Sort(query)
			.Paginate(query)
			.ToListAsync(cancellationToken: cancellationToken);

		int totalItemsCount = await budgets.CountAsync(cancellationToken: cancellationToken);
		var result = new PagedList<BudgetInfo> { Items = items, TotalCount = totalItemsCount };
		return result;
	}
}