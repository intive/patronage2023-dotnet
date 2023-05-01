using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Queries;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;

/// <summary>
/// Get budget details query.
/// </summary>
public record GetBudgetDetails() : IQuery<BudgetDetailsInfo?>
{
	/// <summary>
	/// Budget id to retrive details for.
	/// </summary>
	public Guid Id { get; set; }
}

/// <summary>
/// Get Budget details handler.
/// </summary>
public class GetBudgetDetailsQueryHandler : IQueryHandler<GetBudgetDetails, BudgetDetailsInfo?>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetDetailsQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	public GetBudgetDetailsQueryHandler(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// GetBudgetDetails query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>BudgetDetailsInfo or null.</returns>
	public async Task<BudgetDetailsInfo?> Handle(GetBudgetDetails query, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(query.Id);

		var budget = await this.budgetDbContext.Budget.FindAsync(new object?[] { budgetId }, cancellationToken: cancellationToken);

		return budget is null ? null : BudgetAggregateBudgetDetailsInfoMapper.Map(budget);
	}
}