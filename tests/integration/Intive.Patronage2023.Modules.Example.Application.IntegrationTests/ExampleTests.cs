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
using Intive.Patronage2023.Modules.Example.Application.IntegrationTests;

namespace Intive.Patronage2023.Modules.Example.Application.ExampleTests;

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

		if (dbContext == null)
		{
			throw new ArgumentNullException(nameof(dbContext));
		}

		dbContext.Add(command);

		await dbContext.SaveChangesAsync();

		var query = new GetBudgets();
		if (query == null)
		{
			throw new ArgumentNullException(nameof(query));
		}

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