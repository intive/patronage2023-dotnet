using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.User.Domain.Rules;

/// <summary>
/// User rule.
/// </summary>
public class SuperImportantUserBusinessRule : IBusinessRule
{
	private readonly string name;

	/// <summary>
	/// Initializes a new instance of the <see cref="SuperImportantUserBusinessRule"/> class.
	/// </summary>
	/// <param name="name">User name.</param>
	public SuperImportantUserBusinessRule(string name)
	{
		this.name = name;
	}

	/// <inheritdoc />
	public bool IsBroken() => this.name.StartsWith("brokenRule");
}