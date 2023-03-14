using Bogus;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Modules.Example.Domain;
using Moq;

namespace Intive.Patronage2023.Example.Tests
{
	/// <summary>
	/// Test that check if the method correctly creates an instance of "ExampleAggregate" with the expected values
	/// and then passes that instance to a mocked implementation of "IExampleRepository" to persist it.
	/// The test uses a "Mock" object to simulate the behavior of the "IExampleRepository"
	/// interface and verify that the "Persist" method is called once with the expected values.
	/// </summary>
	public class CreateExampleCommandHandlerTests
	{
		private readonly Mock<IExampleRepository> exampleRepositoryMock;
		private readonly HandleCreateExample handleCreateExample;

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateExampleCommandHandlerTests"/> class.
		/// </summary>
		public CreateExampleCommandHandlerTests()
		{
			this.exampleRepositoryMock = new Mock<IExampleRepository>();
			this.handleCreateExample = new HandleCreateExample(this.exampleRepositoryMock.Object);
		}

		[Fact]
		public async Task Handle_WhenCalled_CreatesExampleAggregateWithCorrectValues()
		{
			// Arrange
			var id = Guid.NewGuid();
			string name = new Faker().Name.FirstName();

			// Act
			await this.handleCreateExample.Handle(new CreateExample(id, name));

			// Assert
			this.exampleRepositoryMock.Verify(
				r => r.Persist(It.Is<ExampleAggregate>(e =>
					e.Id == id &&
					e.Name == name)),
				Times.Once);
		}
	}
}
