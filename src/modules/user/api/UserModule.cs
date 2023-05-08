using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure;

namespace Intive.Patronage2023.Modules.User.Api;

/// <summary>
/// User module.
/// </summary>
public static class UserModule
{
	/// <summary>
	/// Add module services.
	/// </summary>
	/// <param name="services">IServiceCollection.</param>
	/// <param name="configurationManager">ConfigurationManager.</param>
	/// <returns>Updated IServiceCollection.</returns>
	public static IServiceCollection AddUserModule(this IServiceCollection services, ConfigurationManager configurationManager)
	{
		services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();
		return services;
	}

	/// <summary>
	/// Customizes app building process.
	/// </summary>
	/// <param name="app">IApplicationBuilder.</param>
	/// <returns>Updated IApplicationBuilder.</returns>
	public static IApplicationBuilder UseUserModule(this IApplicationBuilder app)
	{
		return app;
	}
}