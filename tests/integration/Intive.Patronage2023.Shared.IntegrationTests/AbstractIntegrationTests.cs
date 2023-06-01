using Hangfire;

using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Intive.Patronage2023.Shared.Infrastructure.Email;
using Intive.Patronage2023.Shared.IntegrationTests.Email;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Shared.IntegrationTests;

/// <summary>
/// Base class used for integration tests.
/// </summary>
[Collection("Database collection")]
public abstract class AbstractIntegrationTests : IDisposable
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AbstractIntegrationTests"/> class.
	/// </summary>
	/// <param name="fixture">The database fixture.</param>
	protected AbstractIntegrationTests(MsSqlTests fixture)
	{
		this.WebApplicationFactory = new CustomWebApplicationFactory(fixture);
	}

	/// <summary>
	/// Application factory.
	/// </summary>
	protected WebApplicationFactory<Api.Program> WebApplicationFactory { get; init; }

	///<summary>
	///Disposes the web application factory used in the test.
	///</summary>
	public void Dispose()
	{
		using (var scope = this.WebApplicationFactory.Services.CreateScope())
		{
			var budgetDbContext = scope.ServiceProvider.GetRequiredService<BudgetDbContext>();

			budgetDbContext.Budget.ExecuteDelete();
			budgetDbContext.Transaction.ExecuteDelete();
		}
		this.WebApplicationFactory.Dispose();
	}

	///<summary>
	///Custom web application factory used in the test to configure the test database connection string.
	///</summary>
	private class CustomWebApplicationFactory : WebApplicationFactory<Api.Program>
	{
		///<summary>
		///The connection string to use when creating instances of the Patronage2023 application.
		///</summary>
		private readonly string connectionString;

		///<summary>
		///Creates a new instance of the CustomWebApplicationFactory class with the specified fixture.
		///</summary>
		///<param name="fixture">The MsSqlTests fixture to use when creating instances of the Patronage2023 application.</param>
		public CustomWebApplicationFactory(MsSqlTests fixture)
		{
			this.connectionString =
				$"Server=localhost,{fixture.MappedPort};Database={MsSqlTests.Database};User Id={MsSqlTests.Username};Password={MsSqlTests.Password};TrustServerCertificate=True";
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			var emailConfiguration = new EmailConfiguration() { SmtpPort = SmtpServerFixture.Port, SmtpServer = "localhost", UseSSL = false };
			var emailConfigDescriptor = new ServiceDescriptor(typeof(IEmailConfiguration), emailConfiguration);
			builder.ConfigureServices(
				services =>
				{
					services.Remove(services.SingleOrDefault(service => typeof(DbContextOptions<BudgetDbContext>) == service.ServiceType)!);
					services.AddDbContext<BudgetDbContext>((_, option) => option.UseSqlServer(this.connectionString));
					services.Remove(services.SingleOrDefault(service => typeof(DbContextOptions<ExampleDbContext>) == service.ServiceType)!);
					services.AddDbContext<ExampleDbContext>((_, option) => option.UseSqlServer(this.connectionString));
					services.Remove(services.SingleOrDefault(service => typeof(IEmailConfiguration) == service.ServiceType)!);
					services.Add(emailConfigDescriptor);
					services.AddHangfire((_, option) => option.UseSqlServerStorage(this.connectionString));
				});
		}
	}

	/// <summary>
	/// Dummy class for collection definition.
	/// </summary>
	[CollectionDefinition("Database collection", DisableParallelization = false)]
	public class DatabaseDefinitionTestFixtureCollection : ICollectionFixture<MsSqlTests>
	{
	}
}