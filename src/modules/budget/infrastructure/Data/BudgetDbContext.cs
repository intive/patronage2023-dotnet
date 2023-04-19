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
	public DbSet<TransactionAggregate> Transaction { get; set; }

	/// <summary>
	/// Domain Event Store DbSet.
	/// </summary>
	public DbSet<DomainEventStore> DomainEventStore { get; set; }

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		////modelBuilder.Entity<TransactionAggregate>().Property(p => p.BudgetId)
		////.HasConversion(
		////v => v.Value,
		////v => new BudgetId(v));

		////modelBuilder.Entity<TransactionAggregate>().Property(p => p.TransactionId)
		////.HasConversion(
		////v => v.Value,
		////v => new TransactionId(v));
		var converter = new ValueConverter<TransactionId, Guid>(
			id => id.Value,
			guid => new TransactionId(guid));

		var converter2 = new ValueConverter<BudgetId, Guid>(
		id => id.Value,
		guid => new BudgetId(guid));

		var entity = modelBuilder.Entity<TransactionAggregate>();

		entity.HasKey(e => e.TransactionId);

		entity.Property(e => e.TransactionId)
			.HasConversion(converter);

		entity.Property(e => e.BudgetId)
			.HasConversion(converter2);

		modelBuilder.ApplyAllConfigurationsFromAssemblies(typeof(BudgetDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}
}