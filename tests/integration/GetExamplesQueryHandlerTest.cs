using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Intive.Patronage2023.Modules.Example.Application.Tests.Example.GettingExamples
{
    public class GetExampleQueryHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCalled_ShouldReturnPagedList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExampleDbContext>()
                .UseInMemoryDatabase(databaseName: "GetExamples_Db")
                .Options;
            var dbContext = new ExampleDbContext(options);
            var example = ExampleHelper.CreateExample();
            dbContext.Example.Add(example);
            await dbContext.SaveChangesAsync();
            var query = new GetExamples();
            var handler = new GetExampleQueryHandler(dbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(example.Id, result.Items.First().Id);
            Assert.Equal(example.Name, result.Items.First().Name);
        }
    }
}
