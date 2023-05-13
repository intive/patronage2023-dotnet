using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain.Rules;

/// <summary>
/// User can't have two budgets with the same name.
/// </summary>
public class BudgetCannotHaveNameEndedWithBrokenRuleBusinessRule : IBusinessRule
{
	private readonly string name;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetCannotHaveNameEndedWithBrokenRuleBusinessRule"/> class.
	/// </summary>
	/// <param name="name">Budget name.</param>
	public BudgetCannotHaveNameEndedWithBrokenRuleBusinessRule(string name)
	{
		this.name = name;
	}

	/// <inheritdoc/>
	public string RuleName => nameof(BudgetCannotHaveNameEndedWithBrokenRuleBusinessRule) + "doesn't meet requirement";

	/// <inheritdoc />
	public bool IsBroken() => this.name.StartsWith("brokenRule");
}