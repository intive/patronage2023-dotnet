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
		builder.HasKey(x => x.Id);
		builder.HasIndex(x => new { x.UserId, x.Name }, "IX_Budget_UserId_Name").IsUnique();
		builder.ToTable("Budget", "Budgets");
		builder.Property(x => x.Id).HasColumnName("Id");
		builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(256);
		builder.Property(x => x.UserId).HasColumnName("UserId");
		builder.Property(x => x.Limit).HasColumnName("Limit");
		builder.OwnsOne(x => x.Limit);
		builder.Property(x => x.Period).HasColumnName("Period");
		builder.OwnsOne(x => x.Period);
		builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
	}
}