namespace Intive.Patronage2023.Modules.Example.Api;

using FluentValidation;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;

/// <summary>
/// Example module.
/// </summary>
public static class ExampleModule
{
    /// <summary>
    /// Add module services.
    /// </summary>
    /// <param name="services">IServiceCollection.</param>
    /// <returns>Updated IServiceCollection.</returns>
    public static IServiceCollection AddExampleModule(this IServiceCollection services)
    {
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