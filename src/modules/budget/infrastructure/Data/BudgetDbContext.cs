using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
	/// Domain Event Store DbSet.
	/// </summary>
	public DbSet<DomainEventStore> DomainEventStore { get; set; }

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		var transactionIdConverter = new ValueConverter<TransactionId, Guid>(
		id => id.Value,
		guid => new TransactionId(guid));

		var budgetIdConverter = new ValueConverter<BudgetId, Guid>(
		id => id.Value,
		guid => new BudgetId(guid));

		var transactionAggregate = modelBuilder.Entity<BudgetTransactionAggregate>();

		transactionAggregate.HasKey(e => e.TransactionId);

		transactionAggregate.Property(e => e.TransactionId)
			.HasConversion(transactionIdConverter);

		transactionAggregate.Property(e => e.BudgetId)
			.HasConversion(budgetIdConverter);

		modelBuilder.Entity<BudgetTransactionAggregate>()
		.HasOne<BudgetAggregate>()
		.WithMany()
		.HasForeignKey(p => p.BudgetId);

		var budgetAggregate = modelBuilder.Entity<BudgetAggregate>();

		budgetAggregate.HasKey(e => e.BudgetId);

		budgetAggregate.Property(e => e.BudgetId)
			.HasConversion(budgetIdConverter);

		modelBuilder.ApplyAllConfigurationsFromAssemblies(typeof(BudgetDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}
}