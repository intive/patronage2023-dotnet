using System.Linq.Dynamic.Core;

using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Application.Extensions;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Application.GettingUsers;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Abstractions.UserContext;

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
	/// Transaction type to filter. Null for all.
	/// </summary>
	public TransactionType? TransactionType { get; init; }

	/// <summary>
	/// Categories type to filter. Empty array or null for all.
	/// </summary>
	public CategoryType[]? CategoryTypes { get; init; }

	/// <summary>
	/// Budget Id.
	/// </summary>
	public BudgetId BudgetId { get; init; }

	/// <summary>
	/// Search text.
	/// </summary>
	public string? Search { get; set; }

	/// <summary>
	/// Sort descriptors.
	/// </summary>
	public List<SortDescriptor>? SortDescriptors { get; set; }
}

/// <summary>
/// Get Budget's Transaction handler.
/// </summary>
public class GetTransactionsQueryHandler : IQueryHandler<GetBudgetTransactions, PagedList<BudgetTransactionInfo>>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IQueryBus queryBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetTransactionsQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	/// <param name="queryBus">Bus that executes query.</param>
	public GetTransactionsQueryHandler(BudgetDbContext budgetDbContext, IQueryBus queryBus)
	{
		this.budgetDbContext = budgetDbContext;
		this.queryBus = queryBus;
	}

	/// <summary>
	/// GetBudgets query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>Paged list of Budgets.</returns>
	public async Task<PagedList<BudgetTransactionInfo>> Handle(GetBudgetTransactions query, CancellationToken cancellationToken)
	{
		var budgets = this.budgetDbContext.Transaction.AsQueryable()
			.For(query.BudgetId)
			.WithType(query.TransactionType)
			.WithCategoryTypes(query.CategoryTypes);

		if (!string.IsNullOrEmpty(query.Search))
		{
			budgets = budgets.Where(x => x.Name.Contains(query.Search));
		}

		var users = (await this.queryBus.Query<GetUsers, PagedList<UserInfoDto>>(new GetUsers()
		{
			PageIndex = 1,
			PageSize = int.MaxValue,
			Search = string.Empty,
			SortDescriptors = new List<SortDescriptor>()
			{
				new SortDescriptor()
				{
					ColumnName = nameof(UserInfoDto.Email),
					SortAscending = true,
				},
			},
		})).Items.ToDictionary(x => x.Email, x => x);

		int totalItemsCount = await budgets
			.CountAsync(cancellationToken: cancellationToken);

		var budgetsOrdered = budgets.Sort(query.SortDescriptors!);
		var items = await budgetsOrdered
			.Paginate(query)
			.MapToTransactionInfoDto()
			.ToListAsync();
		var mappedData = items.MapToTransactionInfo(users).ToList();

		var result = new PagedList<BudgetTransactionInfo> { Items = mappedData, TotalCount = totalItemsCount };
		return result;
	}
}