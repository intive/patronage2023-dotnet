using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Budgets;

/// <summary>
/// Class CreateBudgetInfoAsync.
/// </summary>
public class CreateBudgetInfoAsync
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="CreateBudgetInfoAsync"/> class.
	/// DataService.
	/// </summary>
	/// <param name="budgetDbContext">The DbContext for accessing budget data in the database.</param>
	public CreateBudgetInfoAsync(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// Creates a new budget based on the provided budget information.
	/// If a budget with the same name already exists in the database, a random number is appended to the name.
	/// </summary>
	/// <param name="budget">The budget information used to create the new budget.</param>
	/// <returns>Creates a new budget.</returns>
	public GetBudgetTransferInfo? Create(GetBudgetTransferInfo budget)
	{
		bool isExistingBudget = this.budgetDbContext.Budget.Any(b => b.Name.Equals(budget.Name));
		string budgetName = isExistingBudget ? budget.Name + new Random().Next(100000, 900001) : budget.Name;

		var budgetInfo = new GetBudgetTransferInfo
		{
			Name = budgetName,
			IconName = budget.IconName,
			Description = budget.Description,
			Currency = budget.Currency,
			Value = budget.Value,
			StartDate = budget.StartDate,
			EndDate = budget.EndDate,
		};

		return budgetInfo;
	}
}