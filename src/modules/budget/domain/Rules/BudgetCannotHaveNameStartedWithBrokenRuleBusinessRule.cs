using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain.Rules;

/// <summary>
/// User can't have two budgets with the same name.
/// </summary>
public class BudgetCannotHaveNameStartedWithBrokenRuleBusinessRule : IBusinessRule
{
	private readonly string name;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetCannotHaveNameStartedWithBrokenRuleBusinessRule"/> class.
	/// </summary>
	/// <param name="name">Budget name.</param>
	public BudgetCannotHaveNameStartedWithBrokenRuleBusinessRule(string name)
	{
		this.name = name;
	}

	/// <inheritdoc/>
	public string RuleName => nameof(BudgetCannotHaveNameStartedWithBrokenRuleBusinessRule) + "doesn't meet requirement";

	/// <inheritdoc />
	public bool IsBroken() => this.name.StartsWith("brokenRule");
}