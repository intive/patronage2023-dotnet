using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Moq;
using Xunit.Abstractions;

namespace Intive.Patronage2023.Example.Tests
{
	public class CreateExampleCommandHandlerTests
	{
		private readonly Mock<IExampleRepository> exampleRepositoryMock;
		private readonly HandleCreateExample handleCreateExample;

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
			var name = new Faker().Name.FirstName();

			// Act
			await this.handleCreateExample.Handle(new CreateExample(id, name));

			// Assert

			this.exampleRepositoryMock.Verify(r => r.Persist(It.Is<ExampleAggregate>(e =>
				e.Id == id &&
				e.Name == name)));
		}
	}
}
