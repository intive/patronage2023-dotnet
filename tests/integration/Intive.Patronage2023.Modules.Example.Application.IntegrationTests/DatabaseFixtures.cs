using DotNet.Testcontainers.Builders;

using FluentAssertions;

using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using IContainer = DotNet.Testcontainers.Containers.IContainer;

namespace Intive.Patronage2023.Modules.Example.Application.IntegrationTests;

public class MsSqlTests : IAsyncLifetime
{
	public const string Database = "patronage2023";
	public const string Username = "sa";
	public const string Password = "yourStrong(!)Password";
	public const ushort MsSqlPort = 1433;
	public const ushort MappedPort = 5000;

	public readonly IContainer _mssqlContainer = new ContainerBuilder()
		.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
		.WithPortBinding(MappedPort.ToString(), MsSqlPort.ToString())
		.WithEnvironment("ACCEPT_EULA", "Y")
		.WithEnvironment("SQLCMDUSER", Username)
		.WithEnvironment("SQLCMDPASSWORD", Password)
		.WithEnvironment("MSSQL_SA_PASSWORD", Password)
		.WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("/opt/mssql-tools/bin/sqlcmd", "-Q", "SELECT 1;"))
		.Build();

	public Task InitializeAsync()
	{
		return this._mssqlContainer.StartAsync();
	}

	public Task DisposeAsync()
	{
		return this._mssqlContainer.DisposeAsync().AsTask();
	}
}

public class ExampleTests : IClassFixture<MsSqlTests>, IDisposable
{
	private readonly WebApplicationFactory<Intive.Patronage2023.Api.Program> _webApplicationFactory;

	public ExampleTests(MsSqlTests fixture)
	{
		var clientOptions = new WebApplicationFactoryClientOptions
		{
			AllowAutoRedirect = false
		};
		this._webApplicationFactory = new CustomWebApplicationFactory(fixture);
	}

	[Fact]
	public async Task Handle_WhenCalled_ShouldReturnPagedList()
	{
		// Arrange
		var scope = this._webApplicationFactory.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetService<BudgetDbContext>();
		var command = BudgetAggregate.Create(
			Guid.NewGuid(),
			"example name",
			Guid.NewGuid(),
			new Money(1, (Currency)1),
			new Period(DateTime.Now, DateTime.Now.AddDays(1)),
			"icon",
			"description");

		dbContext?.Add(command);
		await dbContext.SaveChangesAsync();
		var query = new GetBudgets();

		if (dbContext == null)
		{
			throw new ArgumentNullException(nameof(dbContext));
		}

		var handler = new GetBudgetsQueryHandler(dbContext);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(1);
	}

	public void Dispose()
	{
		this._webApplicationFactory.Dispose();
	}

	private class CustomWebApplicationFactory : WebApplicationFactory<Intive.Patronage2023.Api.Program>
	{
		private readonly string connectionString;

		public CustomWebApplicationFactory(MsSqlTests fixture)
		{
			this.connectionString =
				$"Server=localhost,{MsSqlTests.MappedPort};Database={MsSqlTests.Database};User Id={MsSqlTests.Username};Password={MsSqlTests.Password};TrustServerCertificate=True";
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(
				services =>
				{
					services.Remove(services.SingleOrDefault(service => typeof(DbContextOptions<BudgetDbContext>) == service.ServiceType)!);
					services.AddDbContext<BudgetDbContext>((_, option) => option.UseSqlServer(this.connectionString));
					services.Remove(services.SingleOrDefault(service => typeof(DbContextOptions<ExampleDbContext>) == service.ServiceType)!);
					services.AddDbContext<ExampleDbContext>((_, option) => option.UseSqlServer(this.connectionString));
				});
		}
	}
}