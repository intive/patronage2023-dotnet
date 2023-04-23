using Intive.Patronage2023.Shared.Abstractions;

namespace Intive.Patronage2023.Shared.Infrastructure;

/// <summary>
/// Implementation of the IDateTimeService interface.
/// </summary>
public class DateTimeProvider : IDateTimeProvider
{
	private TimeZoneInfo timeZone = TimeZoneInfo.Local;

	/// <inheritdoc/>
	public DateTime UtcNow => DateTime.UtcNow;

	/// <inheritdoc/>
	public DateTime LocalNow => TimeZoneInfo.ConvertTimeFromUtc(this.UtcNow, this.timeZone);

	/// <inheritdoc/>
	public void SetTimeZone(TimeZoneInfo timeZone)
	{
		this.timeZone = timeZone;
	}
}