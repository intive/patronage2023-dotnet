using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data.DataConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Data
{
	/// <summary>
	/// Database context.
	/// </summary>
	public class BudgetDbContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BudgetDbContext"/> class.
		/// </summary>
		/// <param name="options">DbContext options.</param>
		public BudgetDbContext(DbContextOptions<BudgetDbContext> options)
			: base(options)
		{
		}

		/// <summary>
		/// BudgetAggregate DbSet.
		/// </summary>
		public DbSet<BudgetAggregate> Budget { get; set; }

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			new BudgetAggregateEntityConfiguration().Configure(modelBuilder.Entity<BudgetAggregate>());
			base.OnModelCreating(modelBuilder);
		}
	}
}
