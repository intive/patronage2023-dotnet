namespace Intive.Patronage2023.Shared.Abstractions.Domain;

/// <summary>
/// Base interface of repository.
/// </summary>
/// <typeparam name="T">Type of aggregate.</typeparam>
/// <typeparam name="TKey">Type of aggregate key.</typeparam>
public interface IRepository<T, TKey>
{
	/// <summary>
	/// Gets the aggregate by guid identifier.
	/// </summary>
	/// <param name="id">Identifier of aggregate.</param>
	/// <returns>Task that gets Example aggregate.</returns>
	Task<T?> GetById(TKey id);

	/// <summary>
	/// Gets the collection of aggregates by ids collection identifier.
	/// </summary>
	/// <param name="ids">Collection of identifiers of aggregate.</param>
	/// <returns>Task that gets collection aggregates.</returns>
	Task<IList<T>> GetByIds(TKey[] ids);

	/// <summary>
	/// Persists aggregate state.
	/// </summary>
	/// <param name="example">Aggregate.</param>
	/// <returns>Task.</returns>
	Task Persist(T example);

	/// <summary>
	/// Persists aggregate delete state.
	/// </summary>
	/// <param name="aggregate">Aggregate.</param>
	/// <returns>Task.</returns>
	Task Remove(T aggregate);
}