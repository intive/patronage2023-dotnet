using Hangfire;
using Hangfire.Dashboard;

namespace Intive.Patronage2023.Api.Configuration;

/// <summary>
/// HangfireConfiguration is a static class that provides methods for configuring and using Hangfire services.
/// </summary>
public static class HangfireConfiguration
{
	/// <summary>
	/// Adds Hangfire services to the specified IServiceCollection using the provided IConfiguration.
	/// </summary>
	/// <param name="services">The IServiceCollection to add Hangfire services to.</param>
	/// <param name="configuration">The IConfiguration instance to use for Hangfire configuration.</param>
	public static void AddHangfireService(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddHangfire(setup => setup
			.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

		services.AddHangfireServer();

		services.AddScoped<IDashboardAuthorizationFilter, DashboardAuthorizationFilter>();
	}

	/// <summary>
	/// Configures Hangfire Dashboard and adds it to the IApplicationBuilder.
	/// </summary>
	/// <param name="app">The IApplicationBuilder to configure Hangfire Dashboard on.</param>
	public static void UseHangfireService(this IApplicationBuilder app)
	{
		app.UseHangfireDashboard("/hangfire", new DashboardOptions
		{
			DashboardTitle = "Patronage Hangfire Dashboard",
			IsReadOnlyFunc = (Func<DashboardContext, bool>)(_ => true),
			Authorization = new[] { new DashboardAuthorizationFilter() },
		});
	}
}