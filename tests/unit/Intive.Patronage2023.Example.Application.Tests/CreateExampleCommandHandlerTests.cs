using Bogus;

using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Modules.Example.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;
using Moq;

using Xunit;

namespace Intive.Patronage2023.Example.Application.Tests;

/// <summary>
/// Test that checks the behavior of a query handler class "CreateExample".
/// </summary>
public class CreateExampleCommandHandlerTests
{
	private readonly Mock<IRepository<ExampleAggregate, ExampleId>> exampleRepositoryMock;
	private readonly HandleCreateExample handleCreateExample;

	/// <summary>
	/// Initializes a new instance of the <see cref="CreateExampleCommandHandlerTests"/> class.
	/// </summary>
	public CreateExampleCommandHandlerTests()
	{
		this.exampleRepositoryMock = new Mock<IRepository<ExampleAggregate, ExampleId>>();
		this.handleCreateExample = new HandleCreateExample(this.exampleRepositoryMock.Object);
	}

	/// <summary>
	/// Test that check if the method correctly creates an instance of "ExampleAggregate" with the expected values
	/// and then passes that instance to a mocked implementation of "IExampleRepository" to persist it.
	/// The test uses a "Mock" object to simulate the behavior of the "IExampleRepository"
	/// interface and verify that the "Persist" method is called once with the expected values.
	/// </summary>
	[Fact]
	public async void Handle_WhenCalled_ShouldCreatesExampleAggregateWithCorrectValues()
	{
		// Arrange
		var id = Guid.NewGuid();
		var cancellationToken = CancellationToken.None;
		string name = new Faker().Name.FirstName();

		// Act
		await this.handleCreateExample.Handle(new CreateExample(id, name), cancellationToken);

		// Assert
		this.exampleRepositoryMock.Verify(
			r => r.Persist(It.Is<ExampleAggregate>(e =>
				e.Id.Value == id &&
				e.Name == name)),
			Times.Once);
	}
}