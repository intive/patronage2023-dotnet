namespace Intive.Patronage2023.Modules.Example.Domain.Rules;

using Intive.Patronage2023.Shared.Abstractions.Domain;

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

	/// <inheritdoc />
	public bool IsBroken() => this.name.StartsWith("brokenRule");
}