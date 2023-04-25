using FluentAssertions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;
using Xunit;

namespace Intive.Patronage2023.Budget.Application.Tests;

/// <summary>
/// Test that checks the behavior of a query handler class "GetBudgetDetailsQueryHandlerTests".
/// </summary>
public class GetBudgetDetailsQueryHandlerTests
{
	/// <summary>
	/// Test that check if the method throws a "NotImplementedException" when called,
	/// indicating that the implementation of the method is not yet complete.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public void Handle_WhenCalled_ShouldReturnBudgetDetailsInfo()
	{
		// Arrange
		var query = new GetBudgetDetails();
		var cancellationToken = CancellationToken.None;
		var handler = new GetBudgetDetailsQueryHandler(null!); // TODO: Use integration tests db context.

		// Act
		Action act = async () => await handler.Handle(query, cancellationToken);

		// Assert
		act.Should().Throw<NotImplementedException>();
	}
}