using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Example.Domain.Rules;

/// <summary>
/// Example rule.
/// </summary>
public class ExampleCannotHaveNameStartedWithBrokenRuleBusinessRule : IBusinessRule
{
	private readonly string name;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleCannotHaveNameStartedWithBrokenRuleBusinessRule"/> class.
	/// </summary>
	/// <param name="name">Example name.</param>
	public ExampleCannotHaveNameStartedWithBrokenRuleBusinessRule(string name)
	{
		this.name = name;
	}

	/// <inheritdoc/>
	public string RuleName => nameof(ExampleCannotHaveNameStartedWithBrokenRuleBusinessRule) + "doesn't meet requirement";

	/// <inheritdoc />
	public bool IsBroken() => this.name.StartsWith("brokenRule");
}