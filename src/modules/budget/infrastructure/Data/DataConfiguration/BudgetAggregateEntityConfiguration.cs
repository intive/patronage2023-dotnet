using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
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
		builder.HasKey(x => x.Id);
		builder.HasIndex(x => new { x.UserId, x.Name }, "IX_Budget_UserId_Name").IsUnique();
		builder.ToTable("Budget", "Budgets");
		builder.Property(e => e.Id)
			.HasConversion(BudgetConverters.BudgetIdConverter());
		builder.Property(x => x.Id).HasColumnName("Id");
		builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(256);
		builder.Property(x => x.UserId).HasColumnName("UserId");
		builder.OwnsOne(x => x.Limit, limit =>
		{
			limit.Property(p => p.Value).HasColumnName("Value");
			limit.Property(p => p.Currency).HasColumnName("Currency");
		});
		builder.OwnsOne(x => x.Period, period =>
		{
			period.Property(p => p.StartDate).HasColumnName("StartDate");
			period.Property(p => p.EndDate).HasColumnName("EndDate");
		});
		builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
		builder.Property(x => x.Status).HasColumnName("Status").HasConversion<string>().HasDefaultValue(Status.Active);

		builder.HasQueryFilter(b => b.Status == Status.Active);
	}
}