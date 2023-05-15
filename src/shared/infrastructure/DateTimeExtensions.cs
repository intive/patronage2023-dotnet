namespace Intive.Patronage2023.Shared.Infrastructure;

/// <summary>
/// Class that provides date time extensions.
/// </summary>
public static class DateTimeExtensions
{
	/// <summary>
	/// Convert DateTime to ISO8601 format.
	/// </summary>
	/// <param name="dateTime">Date time.</param>
	/// <returns>ISO8601.</returns>
	public static string ToISO8601(this DateTime dateTime) => dateTime.ToUniversalTime().ToString("o");
}