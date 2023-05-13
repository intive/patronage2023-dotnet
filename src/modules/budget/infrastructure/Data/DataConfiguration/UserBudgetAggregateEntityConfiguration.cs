using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Data.DataConfiguration;

/// <summary>
/// Budget Aggregate Configuration.
/// </summary>
internal class UserBudgetAggregateEntityConfiguration : IEntityTypeConfiguration<UserBudgetAggregate>
{
	/// <summary>
	/// Configure method.
	/// </summary>
	/// <param name="builder">builder.</param>
	public void Configure(EntityTypeBuilder<UserBudgetAggregate> builder)
	{
		builder.HasKey(e => e.Id);
		builder.ToTable("UserBudget", "Budgets");

		builder.Property(e => e.Id).HasColumnName("Id");
		builder.Property(e => e.UserId)
			.HasConversion(BudgetConverters.UserIdConverter());
		builder.Property(e => e.BudgetId)
			.HasConversion(BudgetConverters.BudgetIdConverter());

		builder.Property(x => x.UserId).HasColumnName("UserId");
		builder.Property(x => x.BudgetId).HasColumnName("BudgetId");
		builder.Property(x => x.UserRole).HasColumnName("UserRole");
	}
}