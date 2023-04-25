using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Intive.Patronage2023.Modules.Example.Application.IntegrationsTests.DatabaseFixtures;

public class DatabaseFixture : IDisposable
{
	public SqlConnection Db { get; private set; }
	public ExampleDbContext DbContext { get; private set; }
	public string ConnectionString { get; set; }

	public DatabaseFixture()
	{
		// Here should be replaced "connection_string" with a valid connection string that points to the database server
		this.Db = new SqlConnection("connection_string");
		var options = new DbContextOptionsBuilder<ExampleDbContext>()
			.UseSqlServer(this.Db)
			.Options;

		this.DbContext = new ExampleDbContext(options);
	}

	public void Dispose()
	{
		this.Db.Dispose();
		this.DbContext.Dispose();
	}
}

public class GetExampleQueryHandlerTests : IClassFixture<DatabaseFixture>
{
	private readonly DatabaseFixture _fixture;

	public GetExampleQueryHandlerTests(DatabaseFixture fixture)
	{
		this._fixture = fixture;
	}
}
