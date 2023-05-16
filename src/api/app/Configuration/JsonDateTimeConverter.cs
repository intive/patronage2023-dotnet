using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Intive.Patronage2023.Api.Configuration;

/// <summary>
/// Convert <see cref="DateTime"/> to or from JSON.
/// </summary>
public class JsonDateTimeConverter : JsonConverter<DateTime>
{
	private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

	/// <inheritdoc/>
	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (!DateTime.TryParseExact(reader.GetString()!, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
		{
			throw new JsonException($"This date {reader.GetString()} has invalid format.");
		}

		return dateValue;
	}

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, DateTime dateTimeValue, JsonSerializerOptions options) =>
				writer.WriteStringValue(dateTimeValue.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
}