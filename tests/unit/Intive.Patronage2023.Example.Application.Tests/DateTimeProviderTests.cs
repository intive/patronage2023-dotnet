using Intive.Patronage2023.Shared.Abstractions;
using Moq;
using Xunit;

namespace Intive.Patronage2023.Example.Application.Tests;

/// <summary>
/// Test that checks the behavior of a DateTimeProvider.
/// </summary>
public class DateTimeProviderTests
{
	/// <summary>
	/// Test that check if the method "LocalNow" returns the correct date and time when UtcNow has fixed value.
	/// </summary>
	[Fact]
	public void LocalNow_WhenUtcNowHasFixedValue_ShouldReturnLocalTime()
	{
		// Arrange
		var dateTimeProvider = new Mock<IDateTimeProvider>();
		dateTimeProvider.Setup(x => x.UtcNow()).Returns(new DateTime(2021, 07, 20));

		var expectedDateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(dateTimeProvider.Object.UtcNow(), TimeZoneInfo.Local);

		dateTimeProvider.Setup(x => x.LocalNow()).Returns(TimeZoneInfo.ConvertTimeFromUtc(dateTimeProvider.Object.UtcNow(), TimeZoneInfo.Local));

		// Act
		var dateTimeLocal = dateTimeProvider.Object.LocalNow();

		// Assert
		Assert.Equal(expectedDateTimeLocal, dateTimeLocal);
	}
}