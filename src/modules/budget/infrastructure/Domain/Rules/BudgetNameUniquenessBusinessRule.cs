using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Abstractions.Queries;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain.Rules;

/// <summary>
/// Budget name uniqness business rule.
/// </summary>
public class BudgetNameUniquenessBusinessRule : IBusinessRule
{
	private readonly IQueryBus queryBus;
	private readonly BudgetDbContext budgetDbContext;
	private readonly IExecutionContextAccessor executionContextAccessor;
	private readonly string name;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetNameUniquenessBusinessRule"/> class.
	/// </summary>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="budgetDbContext">DB Context.</param>
	/// <param name="executionContextAccessor"> User context accessor.</param>
	/// <param name="name">Budget name.</param>
	public BudgetNameUniquenessBusinessRule(IQueryBus queryBus, BudgetDbContext budgetDbContext, IExecutionContextAccessor executionContextAccessor, string name)
	{
		this.queryBus = queryBus;
		this.name = name;
		this.budgetDbContext = budgetDbContext;
		this.executionContextAccessor = executionContextAccessor;
	}

	/// <inheritdoc />
	public bool IsBroken() => this.budgetDbContext.Budget.Any(x => x.Name == this.name && x.UserId == this.executionContextAccessor.GetUserId());
}