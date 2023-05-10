using FluentValidation;
using Intive.Patronage2023.Modules.User.Application.CreatingUser;
using Intive.Patronage2023.Modules.User.Application.GettingUsers;
using Intive.Patronage2023.Modules.User.Application.SignIn;

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
	/// <returns>Updated IServiceCollection.</returns>
	public static IServiceCollection AddUserModule(this IServiceCollection services)
	{
		services.AddScoped<IValidator<SignInUser>, SignInUserValidator>();
		services.AddScoped<IValidator<CreateUser>, CreateUserValidator>();
		services.AddScoped<IValidator<GetUsers>, GetUsersValidator>();
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