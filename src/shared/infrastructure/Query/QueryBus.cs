using System;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using MediatR;

namespace Intive.Patronage2023.Shared.Infrastructure.Queries
{
	/// <summary>
	/// Query bus implementation.
	/// </summary>
	public class QueryBus : IQueryBus
	{
		private readonly IMediator mediator;

		/// <summary>
		/// Initializes a new instance of the <see cref="QueryBus"/> class.
		/// </summary>
		/// <param name="mediator">Injection of MediatR instance to class.</param>
		public QueryBus(IMediator mediator)
		{
			this.mediator = mediator;
		}

		/// <inheritdoc/>
		public async Task<TResponse> Query<TRequest, TResponse>(TRequest query)
		{
			var result = await this.mediator.Send(query!);

			if (result == null)
			{
				throw new ArgumentNullException();
			}

			return (TResponse)result;
		}
	}
}