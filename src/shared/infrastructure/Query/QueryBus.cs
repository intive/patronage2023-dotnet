using System;
using Intive.Patronage2023.Shared.Abstractions.Attributes;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Shared.Infrastructure.Queries
{
	/// <summary>
	/// Query bus implementation.
	/// </summary>
	[Lifetime(Lifetime = ServiceLifetime.Singleton)]
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
			object? result = await this.mediator.Send(query!);

			if (result == null || result is not TResponse)
			{
				throw new InvalidDataException();
			}

			return (TResponse)result;
		}
	}
}