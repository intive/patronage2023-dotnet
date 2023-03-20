namespace Intive.Patronage2023.Modules.Example.Domain
{
	/// <summary>
	/// Domain Event Store.
	/// </summary>
	public class DomainEventStore
	{
		/// <summary>
		/// Domain Event identifier.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Domain Event data.
		/// </summary>
		public string? Data { get; set; }

		/// <summary>
		/// Domain Event type.
		/// </summary>
		public string? Type { get; set; }

		/// <summary>
		/// Domain Event creation timestamp.
		/// </summary>
		public DateTimeOffset CreatedAt { get; set; }
	}
}