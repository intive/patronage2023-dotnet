using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
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
		builder.ToTable("BudgetTransaction", "Budgets");
		builder.HasKey(x => x.Id);

		builder.HasOne<BudgetAggregate>().WithMany().HasForeignKey(k => k.BudgetId);
		builder.Property(e => e.Id)
			.HasConversion(BudgetConverters.TransactionIdConverter());

		builder.Property(e => e.BudgetId)
			.HasConversion(BudgetConverters.BudgetIdConverter());

		builder.Property(x => x.Id).HasColumnName("Id").HasDefaultValueSql("newsequentialid()").IsRequired();
		builder.Property(x => x.BudgetId).HasColumnName("BudgetId").IsRequired();
		builder.Property(x => x.TransactionType).HasColumnName("TransactionType").HasConversion<string>().IsRequired();
		builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
		builder.Property(x => x.Value).HasColumnName("Value").HasColumnType("decimal(19,4)").IsRequired();
		builder.Property(x => x.CategoryType).HasColumnName("CategoryType").HasConversion<string>().IsRequired();
		builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
		builder.Property(x => x.Status).HasColumnName("Status").HasConversion<string>().HasDefaultValue(Status.Active);

		object value = builder.HasQueryFilter(b => b.Status == Status.Active);
	}
}