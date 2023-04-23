using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace Intive.Patronage2023.Modules.Example.Application.IntegrationsTests.DatabaseFixtures;

    public class DatabaseFixture : IDisposable
    {
        public SqlConnection Db { get; private set; }
        public ExampleDbContext DbContext { get; private set; }
        public ServerVersion ConnectionString { get; internal set; }

        public DatabaseFixture()
        {
            Db = new SqlConnection("MyConnectionString");
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

    public class GetExampleQueryHandlerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public GetExampleQueryHandlerTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
    }