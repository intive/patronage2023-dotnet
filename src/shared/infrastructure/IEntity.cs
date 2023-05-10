namespace Intive.Patronage2023.Shared.Infrastructure;

/// <summary>
/// Entity interface.
/// </summary>
/// <typeparam name="TKey">Id object.</typeparam>
public interface IEntity<TKey>
{
	/// <summary>
	/// Entity id.
	/// </summary>
	public TKey Id { get; }
}