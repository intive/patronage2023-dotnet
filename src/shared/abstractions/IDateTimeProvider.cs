namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// Interface that defines methods related to date and time.
/// </summary>
public interface IDateTimeProvider
{
	/// <summary>
	/// Get UTC date and time.
	/// </summary>
	/// <returns>Utc date and time.</returns>
	DateTime UtcNow { get; }

	/// <summary>
	/// Get local date and time.
	/// </summary>
	/// <returns>Local date and time.</returns>
	DateTime LocalNow { get; }

	/// <summary>
	/// Set the TimeZone.
	/// </summary>
	/// <param name="timeZone">Defines the offset between Utc and local time.</param>
	void SetTimeZone(TimeZoneInfo timeZone);
}