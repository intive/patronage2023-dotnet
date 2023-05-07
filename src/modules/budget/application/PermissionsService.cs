using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;

namespace Intive.Patronage2023.Modules.Budget.Application;

/// <summary>
/// Class provides methods to check user permissions for a budget.
/// </summary>
public class PermissionsService
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="PermissionsService"/> class.
	/// </summary>
	/// <param name="contextAccessor">Instance used to get information about the current user.</param>
	/// <param name="budgetDbContext">Instance used to access the database..</param>
	public PermissionsService(IExecutionContextAccessor contextAccessor, BudgetDbContext budgetDbContext)
	{
		this.contextAccessor = contextAccessor;
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// Checks if the user associated with the current execution context has any permissions for the given budgetId.
	/// It returns true if the user is an admin or has any permissions for the budget, false otherwise.
	/// </summary>
	/// <param name="budgetId">BudgetId.</param>
	/// <returns>Bool.</returns>
	public bool IsPermission(BudgetId budgetId)
	{
		bool isAdmin = false; //// this.contextAccessor.IsUserAdmin(); TODO: uncomment if you need to test if user is Admin.
		var userId = this.contextAccessor.GetUserId();
		bool isPermissions = this.budgetDbContext.UserBudget.AsEnumerable().Any(x => x.UserId.Value == userId && x.BudgetId == budgetId);

		return isAdmin || isPermissions;
	}

	/// <summary>
	/// Checks if the user associated with the current execution context has the specified userRole for the given budgetId.
	/// It returns true if the user is an admin or has the specified userRole for the budget, false otherwise.
	/// </summary>
	/// <param name="budgetId">BudgetId.</param>
	/// <param name="userRole">UserRole.</param>
	/// <returns>Bool.</returns>
	public bool IsPermission(BudgetId budgetId, UserRole userRole)
	{
		bool isAdmin = false; //// this.contextAccessor.IsUserAdmin(); TODO: uncomment if you need to test if user is Admin.
		var userId = this.contextAccessor.GetUserId();
		bool isPermissions = this.budgetDbContext.UserBudget.AsEnumerable().Any(x => x.UserId.Value == userId && x.BudgetId == budgetId);
		var roles = this.budgetDbContext.UserBudget.AsEnumerable().Where(x => x.UserId.Value == userId && x.BudgetId == budgetId).Select(x => x.UserRole).First();

		return isAdmin || (isPermissions && roles == userRole);
	}
}