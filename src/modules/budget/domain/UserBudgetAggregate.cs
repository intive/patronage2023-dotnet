using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// This class represents the relationship between a user and a budget in the context of the application.
/// </summary>
public class UserBudgetAggregate : Aggregate, IEntity<Guid>
{
	/// <summary>
	/// Table Index.
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	/// The "UserId" property is a UserId object that identifies the user.
	/// </summary>
	public UserId UserId { get; set; }

	/// <summary>
	/// The "BudgetId" property is a BudgetId object that identifies the budget.
	/// </summary>
	public BudgetId BudgetId { get; set; }

	/// <summary>
	/// The "Type" property is a UserRole enum that specifies the user's role in relation to the budget.
	/// </summary>
	public UserRole UserRole { get; set; }
}