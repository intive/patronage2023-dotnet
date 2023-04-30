using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain.Rules;

/// <summary>
/// Budget and Budget Transaction Soft Delete business rule.
/// </summary>
public class SuperImportantBudgetBusinessRuleForIsDeleted : IBusinessRule
{
	private readonly bool isDeleted;

	/// <summary>
	/// Initializes a new instance of the <see cref="SuperImportantBudgetBusinessRuleForIsDeleted"/> class.
	/// </summary>
	/// <param name="isDeleted">Budget Soft Delete flag.</param>
	public SuperImportantBudgetBusinessRuleForIsDeleted(bool isDeleted)
	{
		this.isDeleted = isDeleted;
	}

	/// <inheritdoc />
	public bool IsBroken() => this.isDeleted == false;
}