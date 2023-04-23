using FluentValidation;
using Intive.Patronage2023.Api.User.CreatingUser;
using Intive.Patronage2023.Api.User.Queries;

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
	/// <param name="configuration">ConfigurationManager.</param>
	/// <returns>Updated IServiceCollection.</returns>
	public static IServiceCollection AddUserModule(this IServiceCollection services, ConfigurationManager configuration)
	{
		services.AddScoped<IValidator<SignInUser>, SignInUserValidator>();
		services.AddScoped<IValidator<CreateUser>, CreateUserValidator>();
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