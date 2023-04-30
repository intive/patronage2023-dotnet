using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Modules.User.Infrastructure.Data;

/// <summary>
///  provides a new instance of a DbContext for use in design-time scenarios, such as running migrations.
/// </summary>
public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
	/// <inheritdoc/>
	public UserDbContext CreateDbContext(string[] args)
	{
		IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.AddJsonFile("appsettings.Development.json")
			.Build();

		var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
		optionsBuilder.UseSqlServer(configuration.GetConnectionString("DockerDb"));

		return new UserDbContext(optionsBuilder.Options);
	}
}