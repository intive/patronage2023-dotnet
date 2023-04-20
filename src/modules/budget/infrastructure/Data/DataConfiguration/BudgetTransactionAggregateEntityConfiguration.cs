using Intive.Patronage2023.Modules.Budget.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Data.DataConfiguration;

/// <summary>
/// Budget Transaction Aggregate Configuration.
/// </summary>
internal class BudgetTransactionAggregateEntityConfiguration : IEntityTypeConfiguration<BudgetTransactionAggregate>
{
	/// <summary>
	/// Configure method.
	/// </summary>
	/// <param name="builder">builder.</param>
	public void Configure(EntityTypeBuilder<BudgetTransactionAggregate> builder)
	{
		builder.HasKey(x => x.TransactionId);
		builder.ToTable("TransactionStore", "Budgets");
		builder.Property(x => x.TransactionId).HasColumnName("Id").HasDefaultValueSql("newsequentialid()").IsRequired();
		builder.Property(x => x.BudgetId).HasColumnName("BudgetId").IsRequired();
		builder.Property(x => x.TransactionType).HasColumnName("TransactionType").HasConversion<string>().IsRequired();
		builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
		builder.Property(x => x.Value).HasColumnName("Value").HasColumnType("decimal").IsRequired();
		builder.Property(x => x.CategoryType).HasColumnName("CategoryType").HasConversion<string>().IsRequired();
		builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
	}
}