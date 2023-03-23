using FluentValidation;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Modules.Example.Contracts.Events;
using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Intive.Patronage2023.Modules.Example.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Example.Api;

/// <summary>
/// Example module.
/// </summary>
public static class ExampleModule
{
	/// <summary>
	/// Add module services.
	/// </summary>
	/// <param name="services">IServiceCollection.</param>
	/// <param name="configurationManager">ConfigurationManager.</param>
	/// <returns>Updated IServiceCollection.</returns>
	public static IServiceCollection AddExampleModule(this IServiceCollection services, ConfigurationManager configurationManager)
	{
		services.AddDbContext<ExampleDbContext>(options => options.UseSqlServer(configurationManager.GetConnectionString("MigrationsDb")));

		services.AddScoped<IExampleRepository, ExampleRepository>();
		services.AddScoped<IValidator<CreateExample>, CreateExampleValidator>();
		services.AddScoped<IValidator<GetExamples>, GetExamplesValidator>();

		return services;
	}

	/// <summary>
	/// Customizes app building process.
	/// </summary>
	/// <param name="app">IApplicationBuilder.</param>
	/// <returns>Updated IApplicationBuilder.</returns>
	public static IApplicationBuilder UseExampleModule(this IApplicationBuilder app)
	{
		return app;
	}
}