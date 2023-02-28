namespace Intive.Patronage2023.Shared.Abstractions.Queries;

/// <summary>
/// Interface to send queries.
/// </summary>
public interface IQueryBus
{
	/// <summary>
	/// Sends the query to the bus.
	/// </summary>
	/// <typeparam name="TRequest">Type of request.</typeparam>
	/// <typeparam name="TResponse">Type of response.</typeparam>
	/// <param name="query">Query to send.</param>
	/// <returns>Task that represents asynchronous operation which returns query resposne.</returns>
	Task<TResponse> Query<TRequest, TResponse>(TRequest query);
}