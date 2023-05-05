namespace Intive.Patronage2023.Shared.Infrastructure;

/// <summary>
/// Entity id interface.
/// </summary>
/// <typeparam name="TKey">Key.</typeparam>
public interface IEntityId<TKey>
{
	/// <summary>
	/// Entity id.
	/// </summary>
	public TKey Value { get; }
}