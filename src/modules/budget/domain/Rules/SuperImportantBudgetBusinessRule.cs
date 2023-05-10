using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain.Rules;

/// <summary>
/// Budget rule.
/// </summary>
public class SuperImportantBudgetBusinessRule : IBusinessRule
{
	private readonly string name;

	/// <summary>
	/// Initializes a new instance of the <see cref="SuperImportantBudgetBusinessRule"/> class.
	/// </summary>
	/// <param name="name">Budget name.</param>
	public SuperImportantBudgetBusinessRule(string name)
	{
		this.name = name;
	}

	/// <inheritdoc/>
	public string RuleName => nameof(SuperImportantBudgetBusinessRule) + "doesn't meet requirement";

	/// <inheritdoc />
	public bool IsBroken() => this.name.StartsWith("brokenRule");
}