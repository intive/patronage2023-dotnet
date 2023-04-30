using FluentValidation;
;
using Intive.Patronage2023.Modules.User.Domain;
using Intive.Patronage2023.Modules.User.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Intive.Patronage2023.Shared.Infrastructure;

using Microsoft.EntityFrameworkCore;

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
		services.AddDbContext<UserDbContext>(options => options.UseSqlServer(configurationManager.GetConnectionString("AppDb")));

		services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();
		services.AddScoped<IUserRepository, UserRepository>();
		return services;
	}

	/// <summary>
	/// Customizes app building process.
	/// </summary>
	/// <param name="app">IApplicationBuilder.</param>
	/// <returns>Updated IApplicationBuilder.</returns>
	public static IApplicationBuilder UseUserModule(this IApplicationBuilder app)
	{
		app.InitDatabase<UserDbContext>();
		return app;
	}
}