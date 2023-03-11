using System.Diagnostics;
using Intive.Patronage2023.Shared.Abstractions.Events;
using MediatR;

namespace Intive.Patronage2023.Shared.Infrastructure.EventDispachers
{
	/// <summary>
	/// Domain event dispatcher.
	/// </summary>
	public class DomainEventDispatcher : IEventDispatcher<IEvent>
	{
		private readonly IMediator mediator;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomainEventDispatcher"/> class.
		/// Constructor of domain event dispatcher.
		/// </summary>
		/// <param name="mediator">Mediator.</param>
		public DomainEventDispatcher(IMediator mediator)
		{
			this.mediator = mediator;
		}

		/// <inheritdoc/>
		public async Task Publish(List<IEvent> eventsList)
		{
			Debug.WriteLine($"Example create domian event. {nameof(DomainEventDispatcher)}");
			foreach (var element in eventsList)
			{
				await this.mediator.Publish(element);
			}
		}
	}
}
