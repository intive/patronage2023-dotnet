namespace Intive.Patronage2023.Shared.Infrastructure.Domain.OwnedEntities;

/// <summary>
/// Budget limit value object.
/// </summary>
public record BudgetLimit
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetLimit"/> class.
	/// </summary>
	/// <param name="value"> Budget start date. </param>
	/// <param name="currency"> Budget end date. </param>
	public BudgetLimit(decimal value, Currency currency)
	{
		this.Value = value;
		this.Currency = currency;
	}

	/// <summary>
	/// Budget limit value.
	/// </summary>
	public decimal Value { get; init; }

	/// <summary>
	/// Budget limit currency.
	/// </summary>
	public Currency Currency { get; init; }
}