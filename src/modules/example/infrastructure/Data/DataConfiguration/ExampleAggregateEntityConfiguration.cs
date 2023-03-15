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
		}
	}
}
