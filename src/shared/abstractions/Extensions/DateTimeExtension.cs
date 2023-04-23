namespace Intive.Patronage2023.Shared.Abstractions.Extensions;

/// <summary>
/// Define extension methods for DateTime.
/// </summary>
public static class DateTimeExtension
{
	/// <summary>
	/// Converts DateTime object to long representation of unix Timestamp in miliseconds.
	/// </summary>
	/// <param name="dateTime">DateTime object.</param>
	/// <returns>Long type representation of unix Timestamp.</returns>
	public static long ToTimestamp(this DateTime dateTime) => new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
}