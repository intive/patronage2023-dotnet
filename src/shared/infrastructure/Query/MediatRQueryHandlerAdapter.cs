using Intive.Patronage2023.Shared.Abstractions.Queries;
using MediatR;

/// <summary>
/// Implementation of adapter that handles events by CommandHandler.
/// </summary>
/// <typeparam name="T">Type of query.</typeparam>
/// <typeparam name="TResponse">Type of response of query.</typeparam>
public class MediatRQueryHandlerAdapter<T, TResponse> : IRequestHandler<T, TResponse>
where T : IRequest<TResponse>, IQuery<TResponse>
{
	private readonly IQueryHandler<T, TResponse> inner;

	/// <summary>
	/// Initializes a new instance of the <see cref="MediatRQueryHandlerAdapter{T, TResponse}"/> class.
	/// </summary>
	/// <param name="inner">Handler of .</param>
	public MediatRQueryHandlerAdapter(IQueryHandler<T, TResponse> inner)
	{
		this.inner = inner;
	}

	/// <inheritdoc/>
	public Task<TResponse> Handle(T command, CancellationToken cancellationToken)
	{
		return this.inner.Handle(command, cancellationToken);
	}
}