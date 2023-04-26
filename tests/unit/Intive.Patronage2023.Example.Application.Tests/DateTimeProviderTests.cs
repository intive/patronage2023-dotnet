using FluentAssertions;
using Intive.Patronage2023.Shared.Infrastructure;
using Xunit;

namespace Intive.Patronage2023.Example.Application.Tests;

/// <summary>
/// Test that checks the behavior of a DateTimeProvider.
/// </summary>
public class DateTimeProviderTests
{
	/// <summary>
	/// Test that check if the method "SetTimeZone" returns the correct local date and time when TimeZone has fixed value.
	/// </summary>
	[Fact]
	public void SetTimeZone_WhenTimeZoneHasFixedValue_ShouldReturnFixedLocalTime()
	{
		// Arrange
		var dateTimeProvider = new DateTimeProvider();
		var fixedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
		var expectedDateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(dateTimeProvider.UtcNow, fixedTimeZone);
		var precision = TimeSpan.FromMinutes(1);

		// Act
		dateTimeProvider.SetTimeZone(fixedTimeZone);

		// Assert
		dateTimeProvider.LocalNow.Should().BeCloseTo(expectedDateTimeLocal, precision);
	}
}