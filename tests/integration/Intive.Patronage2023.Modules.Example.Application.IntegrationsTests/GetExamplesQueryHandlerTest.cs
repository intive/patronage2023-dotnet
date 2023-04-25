using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Modules.Example.Application.IntegrationsTests.DatabaseFixtures;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
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
		var dbContext = this._fixture.DbContext;
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
