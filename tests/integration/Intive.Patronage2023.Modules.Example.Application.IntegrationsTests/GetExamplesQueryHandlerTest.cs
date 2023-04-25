using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Modules.Example.Application.IntegrationsTests.DatabaseFixtures;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;


namespace Intive.Patronage2023.Modules.Example.Application.IntegrationsTests.GetExamplesQueryHandlerTest;

public class GetExampleQueryHandlerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public GetExampleQueryHandlerTests(DatabaseFixture fixture)
        {
            this._fixture = fixture;
        }

        [Fact]
        public async Task Handle_WhenCalled_ShouldReturnPagedList()
        {
		// Arrange
		var options = new DbContextOptionsBuilder<ExampleDbContext>()
		.UseSqlServer("Server=db;Database=Example;User=sa;Password=S3cur3P@ssW0rd!;MultipleActiveResultSets=True;")
		.Options;



		var dbContext = new ExampleDbContext(options);
            var command = new CreateExample(Guid.NewGuid(), "example name");
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

