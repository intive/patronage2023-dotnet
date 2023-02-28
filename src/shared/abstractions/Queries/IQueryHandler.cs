namespace Intive.Patronage2023.Shared.Abstractions.Queries;

/// <summary>
/// Handles the query..
/// </summary>
/// <typeparam name="T">Type of query.</typeparam>
/// <typeparam name="TResult">Result of the query.</typeparam>
public interface IQueryHandler<in T, TResult>
{
	/// <summary>
	/// Handles the query.
	/// </summary>
	/// <param name="query">Query to handle.</param>
	/// <returns>Task that represents asynchronous operation which returns query resposne.</returns>
	Task<TResult> Handle(T query);
}