using System.Reflection;
using Intive.Patronage2023.Modules.Example.Domain;
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
		/// Apply.
		/// </summary>
		/// <param name="modelBuilder">Model builder.</param>
		/// <param name="assemblies">Assemblies to apply configs from.</param>
		protected void ApplyConfiguration(ModelBuilder modelBuilder, params Assembly[] assemblies)
		{
			foreach (var assembly in assemblies)
			{
				modelBuilder.ApplyConfigurationsFromAssembly(assembly);
			}
		}

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			this.ApplyConfiguration(modelBuilder, typeof(ExampleDbContext).Assembly);
			base.OnModelCreating(modelBuilder);
		}
	}
}
