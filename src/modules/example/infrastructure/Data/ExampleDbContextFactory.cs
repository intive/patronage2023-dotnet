using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Data;

/// <summary>
///  provides a new instance of a DbContext for use in design-time scenarios, such as running migrations.
/// </summary>
public class ExampleDbContextFactory : IDesignTimeDbContextFactory<ExampleDbContext>
{
	/// <inheritdoc/>
	public ExampleDbContext CreateDbContext(string[] args)
	{
		IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.AddJsonFile("appsettings.Development.json")
			.Build();

		var optionsBuilder = new DbContextOptionsBuilder<ExampleDbContext>();
		optionsBuilder.UseSqlServer(configuration.GetConnectionString("DockerDb"));

		return new ExampleDbContext(optionsBuilder.Options);
	}
}