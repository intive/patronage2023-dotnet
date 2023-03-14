using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Shared.Infrastructure
{
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
			services.AddSingleton<IEventDispatcher<IEvent>, DomainEventDispatcher>();
			services.AddTransient(typeof(INotificationHandler<>), typeof(MediatrEventHandlerAdapter<>));
			return services;
		}
	}
}