using FluentAssertions;

using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;

using Xunit;

namespace Intive.Patronage2023.Example.Application.Tests;

/// <summary>
/// Test that checks the behavior of a query handler class "GetExamples".
/// </summary>
public class GetExampleQueryHandlerTests
{
	/// <summary>
	/// Test that check if the method throws a "NotImplementedException" when called,
	/// indicating that the implementation of the method is not yet complete.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public void Handle_WhenCalled_ShouldReturnsPagedListExampleInfo()
	{
		// Arrange
		var query = new GetExamples();
		var cancellationToken = CancellationToken.None;
		var handler = new GetExampleQueryHandler(null!); // TODO: Use integration tests db context.

		// Act
		Action act = async () => await handler.Handle(query, cancellationToken);

		// Assert
		act.Should().Throw<NotImplementedException>();
	}
}