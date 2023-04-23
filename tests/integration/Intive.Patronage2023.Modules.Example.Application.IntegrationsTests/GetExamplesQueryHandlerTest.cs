using System;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Modules.Example.Application.IntegrationsTests.DatabaseFixtures;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Intive.Patronage2023.Modules.Example.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using DotNet.Testcontainers.Configurations;


namespace Intive.Patronage2023.Modules.Example.Application.IntegrationsTests.GetExamplesQueryHandlerTest
{
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
            var options = new DbContextOptionsBuilder<ExampleDbContext>()
                .UseMySql(_fixture.ConnectionString)
                .Options;
            var dbContext = new ExampleDbContext(options);
            var command = new CreateExample(Guid.NewGuid(), "example name");
            var example = ExampleAggregate.Create(command.Id, command.Name);
            dbContext.Example.Add(example);
            await dbContext.SaveChangesAsync();
            var query = new GetExamples();
            var handler = new GetExampleQueryHandler(dbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items[0].Should().BeEquivalentTo(example, options => options.ExcludingMissingMembers());

        }
    }
}

