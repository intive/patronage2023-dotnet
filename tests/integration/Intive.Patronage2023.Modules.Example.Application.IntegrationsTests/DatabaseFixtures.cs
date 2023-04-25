using FluentAssertions;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Intive.Patronage2023.Modules.Example.Application.IntegrationsTests.DatabaseFixtures;

public class GetExampleQueryHandlerTests : IClassFixture<DatabaseFixture>
{
	private readonly DatabaseFixture _fixture;

	public GetExampleQueryHandlerTests(DatabaseFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public async Task Handle_WhenCalled_ShouldReturnPagedList()
	{
		// Arrange
		var dbContext = _fixture.DbContext;
		var command = new CreateExample(Guid.NewGuid(), "example name");
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
}

public class DatabaseFixture : IDisposable
{
	public SqlConnection Db { get; private set; }
	public ExampleDbContext DbContext { get; private set; }

	public DatabaseFixture()
	{
		string saPassword = Environment.GetEnvironmentVariable("S3cur3P@ssW0rd!");
		string connectionString = $"Server=localhost,1433;Database=Keycloak;User Id=sa;Password={saPassword};";
		Db = new SqlConnection(connectionString);
		var options = new DbContextOptionsBuilder<ExampleDbContext>()
			.UseSqlServer(Db)
			.Options;

		DbContext = new ExampleDbContext(options);
	}

	public void Dispose()
	{
		Db.Dispose();
		DbContext.Dispose();
	}
}