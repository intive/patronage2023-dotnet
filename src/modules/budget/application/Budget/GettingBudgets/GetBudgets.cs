using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;

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
	public string Search { get; set; } = null!;

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
	private readonly PermissionsService permissionsService;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetsQueryHandler"/> class.
	/// </summary>
	/// <param name="permissionsService">Permissions Service.</param>
	public GetBudgetsQueryHandler(PermissionsService permissionsService)
	{
		this.permissionsService = permissionsService;
	}

	/// <summary>
	/// GetBudgets query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>Paged list of Budgets.</returns>
	public async Task<PagedList<BudgetInfo>> Handle(GetBudgets query, CancellationToken cancellationToken)
	{
		return await this.permissionsService.GetBudgets(query, cancellationToken);
	}
}