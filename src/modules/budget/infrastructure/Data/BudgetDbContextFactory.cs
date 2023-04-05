using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Data;

/// <summary>
///  provides a new instance of a DbContext for use in design-time scenarios, such as running migrations.
/// </summary>
public class BudgetDbContextFactory : IDesignTimeDbContextFactory<BudgetDbContext>
{
	/// <inheritdoc/>
	public BudgetDbContext CreateDbContext(string[] args)
	{
		IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.AddJsonFile("appsettings.Development.json")
			.Build();

		var optionsBuilder = new DbContextOptionsBuilder<BudgetDbContext>();
		optionsBuilder.UseSqlServer(configuration.GetConnectionString("DockerDb"));

		return new BudgetDbContext(optionsBuilder.Options);
	}
}