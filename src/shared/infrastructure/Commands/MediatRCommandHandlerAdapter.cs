using Intive.Patronage2023.Shared.Abstractions.Commands;
using MediatR;

/// <summary>
/// Implementation of adapter that handles events by CommandHandler.
/// </summary>
/// <typeparam name="T">Type of event to handle.</typeparam>
public class MediatRCommandHandlerAdapter<T> : IRequestHandler<T>
where T : IRequest, ICommand
{
	private readonly ICommandHandler<T> inner;

	/// <summary>
	/// Initializes a new instance of the <see cref="MediatRCommandHandlerAdapter{T}"/> class.
	/// </summary>
	/// <param name="inner">Handler of .</param>
	public MediatRCommandHandlerAdapter(ICommandHandler<T> inner)
	{
		this.inner = inner;
	}

	/// <inheritdoc/>
	public Task Handle(T command, CancellationToken cancellationToken)
	{
		return this.inner.Handle(command, cancellationToken);
	}
}