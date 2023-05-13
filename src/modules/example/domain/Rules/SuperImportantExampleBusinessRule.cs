using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Example.Domain.Rules;

/// <summary>
/// Example rule.
/// </summary>
public class SuperImportantExampleBusinessRule : IBusinessRule
{
	private readonly string name;

	/// <summary>
	/// Initializes a new instance of the <see cref="SuperImportantExampleBusinessRule"/> class.
	/// </summary>
	/// <param name="name">Example name.</param>
	public SuperImportantExampleBusinessRule(string name)
	{
		this.name = name;
	}

	/// <inheritdoc/>
	public string RuleName => nameof(SuperImportantExampleBusinessRule) + "doesn't meet requirement";

	/// <inheritdoc />
	public bool IsBroken() => this.name.StartsWith("brokenRule");
}