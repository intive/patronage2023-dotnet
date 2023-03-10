using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data.DataConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Data
{
	/// <summary>
	/// Database context.
	/// </summary>
	public class AppDbContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AppDbContext"/> class.
		/// </summary>
		/// <param name="options">DbContext options.</param>
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}

		/// <summary>
		/// ExampleAggregate DbSet.
		/// </summary>
		public DbSet<ExampleAggregate> ExampleAggregates { get; set; }

		/// <summary>
		/// OnModelCreating override.
		/// </summary>
		/// <param name="modelBuilder">ModelBuilder.</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			new ExampleAggregateEntityConfiguration().Configure(modelBuilder.Entity<ExampleAggregate>());
			base.OnModelCreating(modelBuilder);
		}
	}
}
