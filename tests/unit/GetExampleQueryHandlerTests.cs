using FluentAssertions;
using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;

namespace Intive.Patronage2023.Example.Tests
{
	/// <summary>
	/// Test that check if the method throws a "NotImplementedException" when called,
	/// indicating that the implementation of the method is not yet complete.
	/// </summary>
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