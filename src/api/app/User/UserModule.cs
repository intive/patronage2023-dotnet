using FluentValidation;

namespace Intive.Patronage2023.Api.User.CreatingUser;

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
	public static IServiceCollection AddUserModule(this IServiceCollection services, ConfigurationManager configurationManager)
	{
		services.AddScoped<IValidator<CreateUser>, CreateUserValidator>();
		return services;
	}
}