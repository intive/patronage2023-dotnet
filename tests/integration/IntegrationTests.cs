using System.Xml;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DotNet.Testcontainers.Containers;
using MyProject.MyProject.Tests;

namespace MyProject
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<MyEntity> MyEntities { get; set; }
    }

    public class SharedDatabaseFixture : IDisposable
    {
        private readonly IDockerContainer container;

        public DbContextOptions<MyDbContext> Options { get; }

        public SharedDatabaseFixture()
        {
            container = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                .WithExposedPort(1433)
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("SA_PASSWORD", "MyPassw0rd!")
                .Build();

            container.StartAsync().GetAwaiter().GetResult();

            Options = new DbContextOptionsBuilder<MyDbContext>()
                .UseSqlServer($"Server=localhost,{container.GetMappedPort(1433)};Database=my_database;User=sa;Password=MyPassw0rd!")
                .Options;
        }

        public void Dispose()
        {
            container.StopAsync().GetAwaiter().GetResult();
            container.DisposeAsync();
        }
    }

    public class MyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    namespace MyProject.Tests
    {
        public class MyTests : IClassFixture<SharedDatabaseFixture>
        {
            private readonly SharedDatabaseFixture databaseFixture;

            public MyTests(SharedDatabaseFixture databaseFixture)
            {
                this.databaseFixture = databaseFixture;
            }

            [Fact]
            public void Test1()
            {
                using (var context = new MyDbContext(databaseFixture.Options))
                {
                    context.Database.EnsureCreated();

                    var entity = new MyEntity { Name = "Test" };
                    context.MyEntities.Add(entity);
                    context.SaveChanges();

                    var count = context.MyEntities.Count();
                    Assert.Equal(1, count);
                }
            }
        }
    }
}
