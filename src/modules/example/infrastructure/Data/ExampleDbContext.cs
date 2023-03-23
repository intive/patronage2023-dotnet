using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data.DataConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Data
{
	/// <summary>
	/// Database context.
	/// </summary>
	public class ExampleDbContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExampleDbContext"/> class.
		/// </summary>
		/// <param name="options">DbContext options.</param>
		public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
			: base(options)
		{
		}

		/// <summary>
		/// ExampleAggregate DbSet.
		/// </summary>
		public DbSet<ExampleAggregate> Example { get; set; }

		/// <summary>
		/// Domain Event Store DbSet.
		/// </summary>
		public DbSet<DomainEventStore> DomainEventStore { get; set; }

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			new ExampleAggregateEntityConfiguration().Configure(modelBuilder.Entity<ExampleAggregate>());
			new DomainEventStoreEntityConfiguration().Configure(modelBuilder.Entity<DomainEventStore>());
			base.OnModelCreating(modelBuilder);
		}
	}
}