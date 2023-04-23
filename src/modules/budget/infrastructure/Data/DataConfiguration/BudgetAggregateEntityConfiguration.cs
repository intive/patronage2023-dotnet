using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Data.DataConfiguration;

/// <summary>
/// Budget Aggregate Configuration.
/// </summary>
internal class BudgetAggregateEntityConfiguration : IEntityTypeConfiguration<BudgetAggregate>
{
	/// <summary>
	/// Configure method.
	/// </summary>
	/// <param name="builder">builder.</param>
	public void Configure(EntityTypeBuilder<BudgetAggregate> builder)
	{
		builder.HasKey(x => x.BudgetId);
		builder.ToTable("Budget", "Budgets");
		builder.Property(e => e.BudgetId)
			.HasConversion(BudgetConverters.BudgetIdConverter());
		builder.Property(x => x.BudgetId).HasColumnName("Id");
		builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(256);
		builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
	}
}