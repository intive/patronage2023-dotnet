using Intive.Patronage2023.Modules.Budget.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Data.DataConfiguration;

/// <summary>
/// Budget Aggregate Configuration.
/// </summary>
internal class TransactionAggregateEntityConfiguration : IEntityTypeConfiguration<TransactionAggregate>
{
	/// <summary>
	/// Configure method.
	/// </summary>
	/// <param name="builder">builder.</param>
	public void Configure(EntityTypeBuilder<TransactionAggregate> builder)
	{
		builder.HasKey(x => x.Id);
		builder.ToTable("Transaction", "Budgets");
		builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
		builder.HasOne(x => x.BudgetAggregate);
		builder.Property(x => x.BudgetId == x.BudgetAggregate.Id).HasColumnName("BudgetId").IsRequired();
		builder.Property(x => x.Value).HasColumnName("Value").HasColumnType("decimal").IsRequired();
		builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
		builder.Property(x => x.TransactionType).HasColumnName("TransactionType").IsRequired();
		builder.Property(x => x.CategoryType).HasColumnName("CategoryType").IsRequired();
		builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
	}
}