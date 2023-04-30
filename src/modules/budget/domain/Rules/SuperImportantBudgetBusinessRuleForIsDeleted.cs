using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain.Rules;

/// <summary>
/// Budget rule.
/// </summary>
public class SuperImportantBudgetBusinessRuleForIsDeleted : IBusinessRule
{
	private readonly bool isDeleted;

	/// <summary>
	/// Initializes a new instance of the <see cref="SuperImportantBudgetBusinessRuleForIsDeleted"/> class.
	/// </summary>
	/// <param name="isDeleted">Budget flag.</param>
	public SuperImportantBudgetBusinessRuleForIsDeleted(bool isDeleted)
	{
		this.isDeleted = isDeleted;
	}

	/// <inheritdoc />
	/// TODO: sprawdzić czy może tak zostać, ewentualnie enum
	public bool IsBroken() => this.isDeleted == false;
}