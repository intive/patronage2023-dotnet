using Intive.Patronage2023.Modules.Example.Contracts.Events;
using Intive.Patronage2023.Modules.Example.Infrastructure.Domain.EventHandlers;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

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
	/// <returns>Updated IServiceCollection.</returns>
	public static IServiceCollection AddExampleModule(this IServiceCollection services)
	{
		services.AddSingleton<IDomainEventHandler<ExampleCreatedDomainEvent>, ExampleCreatedDomainEventHandler>();
		services.AddSingleton<IDomainEventHandler<ExampleNameUpdatedDomainEvent>, ExampleNameUpdatedDomainEventHandler>();
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