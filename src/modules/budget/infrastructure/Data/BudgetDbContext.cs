using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Intive.Patronage2023.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Data;

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

	/// <summary>
	/// TransactionAggregate DbSet.
	/// </summary>
	public DbSet<BudgetTransactionAggregate> Transaction { get; set; }

	/// <summary>
	/// This DbSet represents a database table that will store instances of the UserBudgetAggregate entity.
	/// </summary>
	public virtual DbSet<UserBudgetAggregate> UserBudget { get; set; }

	/// <summary>
	/// This DbSet represents a database table that will store instances of the TransactionCategoryAggregate entity.
	/// </summary>
	public DbSet<TransactionCategoryAggregate> BudgetTransactionCategory { get; set; }

	/// <summary>
	/// Domain Event Store DbSet.
	/// </summary>
	public DbSet<DomainEventStore> DomainEventStore { get; set; }

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyAllConfigurationsFromAssemblies(typeof(BudgetDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}
}