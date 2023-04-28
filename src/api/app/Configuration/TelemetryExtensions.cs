using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace Intive.Patronage2023.Api.Configuration;

/// <summary>
/// Class that extends the telemetry in the api.
/// </summary>
public static class TelemetryExtensions
{
	/// <summary>
	/// Extension method that adds the telemetry to the api.
	/// </summary>
	/// <param name="builder">Builder that configures web api..</param>
	/// <returns>The services.</returns>
	public static IServiceCollection AddTelemetry(this WebApplicationBuilder builder)
	{
		var settings = builder.Configuration.GetSection("ApplicationInsights").Get<ApplicationInsightsServiceOptions?>();
		if (settings is not null && !string.IsNullOrEmpty(settings.ConnectionString))
		{
			builder.Services.AddApplicationInsightsTelemetry(settings);
			builder.Logging.AddApplicationInsights(
				configureTelemetryConfiguration: (config) =>
				config.ConnectionString = settings.ConnectionString,
				configureApplicationInsightsLoggerOptions: (options) => { });
			builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("category", LogLevel.Trace);
		}

		return builder.Services;
	}
}