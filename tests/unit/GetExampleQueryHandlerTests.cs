using FluentAssertions;
using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;

namespace Intive.Patronage2023.Example.Tests
{
	public class GetExampleQueryHandlerTests
	{
		[Fact]
		public async Task Handle_WhenCalled_ReturnsPagedListExampleInfo()
		{
			// Arrange
			var query = new GetExamples();
			var handler = new HandleGetExamples();

			// Act
			Action act = () => handler.Handle(query);

			// Assert
			act.Should().Throw<NotImplementedException>();
		}
	}
}