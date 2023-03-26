using Intive.Patronage2023.Shared.Abstractions.Attributes;
using Intive.Patronage2023.Shared.Abstractions.Commands;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Shared.Infrastructure.Commands.CommandBus;

/// <summary>
/// Command Bus implementation.
/// </summary>
[Lifetime(Lifetime = ServiceLifetime.Singleton)]
public class CommandBus : ICommandBus
{
	private readonly IMediator mediator;

	/// <summary>
	/// Initializes a new instance of the <see cref="CommandBus"/> class.
	/// </summary>
	/// <param name="mediator">MediatR injection.</param>
	public CommandBus(IMediator mediator)
	{
		this.mediator = mediator;
	}

	/// <inheritdoc/>
	public Task Send<TCommand>(TCommand command)
		where TCommand : class
	{
		return this.mediator.Send(command);
	}
}