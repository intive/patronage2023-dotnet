using FluentValidation;
using Intive.Patronage2023.Modules.Example.Api.User.CreatingUser;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Example.Api.User;

/// <summary>
/// User Module.
/// </summary>
public static class UserModule
{
	/// <summary>
	/// Add module services.
	/// </summary>
	/// <param name="services">IServiceCollection.</param>
	/// <param name="configurationManager">ConfigurationManager.</param>
	/// <returns>Updated IServiceCollection.</returns>
	public static IServiceCollection AddExampleModule(this IServiceCollection services, ConfigurationManager configurationManager)
	{
		services.AddDbContext<ExampleDbContext>(options => options.UseSqlServer(configurationManager.GetConnectionString("AppDb")));
		services.AddScoped<IValidator<CreateUser>, CreateUserValidator>();
		return services;
	}
}