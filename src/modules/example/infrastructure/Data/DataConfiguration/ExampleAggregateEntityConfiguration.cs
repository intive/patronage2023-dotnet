using Intive.Patronage2023.Modules.Example.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Data.DataConfiguration
{
	/// <summary>
	/// Example Aggregate Configuration.
	/// </summary>
	internal class ExampleAggregateEntityConfiguration : IEntityTypeConfiguration<ExampleAggregate>
	{
		/// <summary>
		/// Configure method.
		/// </summary>
		/// <param name="builder">builder.</param>
		public void Configure(EntityTypeBuilder<ExampleAggregate> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("Example", "Examples");
			builder.Property(x => x.Id).HasColumnName("Id");
			builder.Property(x => x.Name).HasColumnName("Name");
			builder.Property(x => x.Name).HasMaxLength(256);
			builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
		}
	}
}