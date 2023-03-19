using System;
using MediatR;

namespace Intive.Patronage2023.Shared.Abstractions.Commands
{
	/// <summary>
	/// Command Bus implementation.
	/// </summary>
	public class CommandBus : ICommandBus
	{
		private readonly Mediator mediator;

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandBus"/> class.
		/// </summary>
		/// <param name="mediator">MediatR injection.</param>
		public CommandBus(Mediator mediator)
		{
			this.mediator = mediator;
		}

		/// <inheritdoc/>
		public async Task<object> Send<TCommand>(TCommand command)
			where TCommand : class
		{
			return await this.mediator.Send(command);
		}
	}
}