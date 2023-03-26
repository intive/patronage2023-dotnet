using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Shared.Infrastructure.Domain;

/// <summary>
/// BusinessRuleValidationException.
/// </summary>
public class BusinessRuleValidationException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BusinessRuleValidationException"/> class.
	/// </summary>
	/// <param name="rule">Broken rule.</param>
	public BusinessRuleValidationException(IBusinessRule rule)
	{
		throw new NotImplementedException();
	}
}