using FluentAssertions;
using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Intive.Patronage2023.Modules.Example.Application.IntegrationsTests;

public class ExampleAggregate
{
	

	public int Id { get; set; }
	public string Name { get; set; }

	public ExampleAggregate(int id, string name)
	{
		this.Id = id;
		this.Name = name;
	}
}

public class GetExampleQueryHandlerTests
{
	[Fact]
	public async Task Handle_ValidQuery_ReturnsExamples()
	{
		// Arrange
		var options = new DbContextOptionsBuilder<ExampleDbContext>()
			.UseInMemoryDatabase(databaseName: "ExampleDatabase")
			.Options;
		var dbContext = new ExampleDbContext(options);
		var examples = new List<ExampleAggregate>
			{
				new ExampleAggregate(1, "Example 1"),
				new ExampleAggregate(2, "Example 2"),
				new ExampleAggregate(3, "Example 3")
			};

		dbContext.Example.AddRange((IEnumerable<Domain.ExampleAggregate>)examples);
		dbContext.SaveChanges();
		var query = new GetExamples();
		var handler = new GetExampleQueryHandler(dbContext);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(examples.Count);
		result.Items.Should().BeEquivalentTo(examples, options => options.ExcludingMissingMembers());
	}
}

