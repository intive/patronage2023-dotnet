using Intive.Patronage2023.Modules.Budget.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Data.DataConfiguration;

/// <summary>
/// Transaction Aggregate Configuration.
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
		builder.ToTable("TransactionStore", "Budgets");
		builder.Property(x => x.Id).HasColumnName("Id").HasDefaultValueSql("newsequentialid()").IsRequired();
		builder.Property(x => x.BudgetId).HasColumnName("BudgetId").IsRequired();
		builder.Property(x => x.TransactionType).HasColumnName("TransactionType").HasConversion<string>().IsRequired();
		builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
		builder.Property(x => x.Value).HasColumnName("Value").HasColumnType("decimal").IsRequired();
		builder.Property(x => x.CategoryType).HasColumnName("CategoryType").HasConversion<string>().IsRequired();
		builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
	}
}