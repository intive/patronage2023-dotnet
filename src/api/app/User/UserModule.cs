using FluentValidation;

namespace Intive.Patronage2023.Api.User;

/// <summary>
/// User Module.
/// </summary>
public static class UserModule
{
	/// <summary>
	/// Add module services.
	/// </summary>
	/// <param name="services">IServiceCollection.</param>
	/// <returns>Updated IServiceCollection.</returns>
	public static IServiceCollection AddUserModule(this IServiceCollection services)
	{
		services.AddScoped<IValidator<SignInUser>, SignInUserValidator>();
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