using Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Interface of repository for BudgetAggregate.
/// </summary>
public interface IBudgetRepository : IRepository<BudgetAggregate, Guid>
{
	/// <summary>
	/// Checks if budget of given name exists.
	/// </summary>
	/// <param name="name">Budget name.</param>
	/// <returns>True if exists.</returns>
	bool ExistsByName(string name);
}