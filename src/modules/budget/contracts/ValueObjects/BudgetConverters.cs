using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

/// <summary>
/// Class which holds method for Id conversion.
/// </summary>
public static class BudgetConverters
{
	/// <summary>
	/// Converter which changes TransactionId to Guid.
	/// </summary>
	/// <returns>Returns Converted TransactionId to guid.</returns>
	public static ValueConverter TransactionIdConverter() => new ValueConverter<TransactionId, Guid>(
		id => id.Value,
		guid => new TransactionId(guid));

	/// <summary>
	/// Converter which changes BudgetId to Guid.
	/// </summary>
	/// <returns>Returns Converted BudgetId to guid.</returns>
	public static ValueConverter BudgetIdConverter() => new ValueConverter<BudgetId, Guid>(
		id => id.Value,
		guid => new BudgetId(guid));
}