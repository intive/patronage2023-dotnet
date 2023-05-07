using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Intive.Patronage2023.Modules.Example.Contracts.ValueObjects;

/// <summary>
/// Class which holds method for Id conversion.
/// </summary>
public class ExampleConverters
{
	/// <summary>
	/// Converter which changes ExampleId to Guid.
	/// </summary>
	/// <returns>Returns Converted ExampleId to guid.</returns>
	public static ValueConverter ExampleIdConverter() => new ValueConverter<ExampleId, Guid>(
		id => id.Value,
		guid => new ExampleId(guid));
}