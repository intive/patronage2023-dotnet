namespace Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;

/// <summary>
/// Budget limit value object.
/// </summary>
public record Money
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Money"/> class.
	/// </summary>
	/// <param name="value"> Budget start date. </param>
	/// <param name="currency"> Budget end date. </param>
	public Money(decimal value, Currency currency)
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