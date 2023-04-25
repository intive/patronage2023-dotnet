namespace Intive.Patronage2023.Shared.Abstractions.Domain;

/// <summary>
/// Business rule interface.
/// </summary>
public interface IBusinessRule
{
	/// <summary>
	/// Check business rule.
	/// </summary>
	/// <returns>Information if rule has been broken.</returns>
	bool IsBroken();
}