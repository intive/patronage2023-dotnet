using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure.Commands;
using Intive.Patronage2023.Shared.Infrastructure.Email;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Shared.Infrastructure;

/// <summary>
/// Shared module.
/// </summary>
public static class SharedModule
{
	/// <summary>
	/// Add module services.
	/// </summary>
	/// <param name="services">IServiceCollection.</param>
	/// <returns>Updated IServiceCollection.</returns>
	public static IServiceCollection AddSharedModule(this IServiceCollection services)
	{
		services.AddTransient(typeof(INotificationHandler<>), typeof(MediatrEventHandlerAdapter<>));
		services.AddTransient(typeof(IRequestHandler<,>), typeof(MediatRQueryHandlerAdapter<,>));
		services.AddTransient(typeof(IRequestHandler<>), typeof(MediatRCommandHandlerAdapter<>));
		services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
		services.AddSingleton<IEmailConfiguration, EmailConfiguration>();
		services.AddTransient<IEmailService, EmailService>();

		return services;
	}
}