namespace Intive.Patronage2023.Modules.Example.Application.IntegrationsTests;

using System.Data.Common;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class MsSqlTests : IAsyncLifetime
{
	public const string Database = "patronage2023";
	public const string Username = "sa";
	public const string Password = "yourStrong(!)Password";
	public const ushort MsSqlPort = 1433;

	public readonly IContainer _mssqlContainer = new ContainerBuilder()
		.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
		.WithPortBinding("1434", MsSqlPort.ToString())
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
		var clientOptions = new WebApplicationFactoryClientOptions();
		clientOptions.AllowAutoRedirect = false;
		this._webApplicationFactory = new CustomWebApplicationFactory(fixture);
	}

	[Fact]
	public async Task Handle_WhenCalled_ShouldReturnPagedList()
	{
		// Arrange
		var scope = this._webApplicationFactory.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetService<ExampleDbContext>();
		var command = ExampleAggregate.Create(Guid.NewGuid(), "example name");
		dbContext.Add(command);
		await dbContext.SaveChangesAsync();
		var query = new GetExamples();
		var handler = new GetExampleQueryHandler(dbContext);

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
		private readonly string _connectionString;

		public CustomWebApplicationFactory(MsSqlTests fixture)
		{
			this._connectionString =
				$"Server=localhost,1434;Database={MsSqlTests.Database};User Id={MsSqlTests.Username};Password={MsSqlTests.Password};TrustServerCertificate=True";
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(
				services =>
				{
					services.Remove(services.SingleOrDefault(service => typeof(DbContextOptions<ExampleDbContext>) == service.ServiceType));
					services.Remove(services.SingleOrDefault(service => typeof(DbConnection) == service.ServiceType));
					services.AddDbContext<ExampleDbContext>((_, option) => option.UseSqlServer(this._connectionString));
				});
		}
	}
}