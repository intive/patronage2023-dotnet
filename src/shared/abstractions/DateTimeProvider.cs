namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// Implementation of the IDateTimeService interface.
/// </summary>
public class DateTimeProvider : IDateTimeProvider
{
	private TimeZoneInfo timeZone = TimeZoneInfo.Local;

	/// <inheritdoc/>
	public DateTime UtcNow() => DateTime.UtcNow;

	/// <inheritdoc/>
	public DateTime LocalNow()
	{
		return TimeZoneInfo.ConvertTimeFromUtc(this.UtcNow(), this.timeZone);
	}

	/// <inheritdoc/>
	public void SetTimeZone(TimeZoneInfo timeZone)
	{
		this.timeZone = timeZone;
	}
}