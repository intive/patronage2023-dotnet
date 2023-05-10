using Bogus;
using FluentAssertions;
using Intive.Patronage2023.Modules.User.Application.GettingUsers;
using Intive.Patronage2023.Modules.User.Domain;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Intive.Patronage2023.User.Application.Tests;

/// <summary>
/// Test that checks the behavior of a query handler class "GetBudgetDetailsQueryHandlerTests".
/// </summary>
public class GetUsersQueryHandlerTests
{
	private readonly Mock<IKeycloakService> keycloakServiceMock;
	private readonly GetUsersQueryHandler handler;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetUsersQueryHandlerTests"/> class.
	/// </summary>
	public GetUsersQueryHandlerTests()
	{
		this.keycloakServiceMock = new Mock<IKeycloakService>();
		this.FakeTokenResponse();
		this.handler = new GetUsersQueryHandler(this.keycloakServiceMock.Object);
	}

	/// <summary>
	/// Test that checks if the query returns expected values.
	/// </summary>
	[Fact]
	public async Task Handle_WhenCalled_ShouldReturnListOfUsers()
	{
		// Arrange
		int pageSize = 10;
		var users = this.SetupUsers(pageSize);

		var query = new GetUsers() { PageSize = pageSize };

		// Act
		var response = await this.handler.Handle(query, CancellationToken.None);

		var fakedMails = users.Select(x => x.Email).ToList();
		var responseMails = response.Items.Select(x => x.Email).ToList();

		// Assert
		responseMails.Should().Equal(fakedMails);
	}

	/// <summary>
	/// Test that checks if the query returns sorted values.
	/// </summary>
	[Fact]
	public async Task Handle_WhenCalledWithValidSortParameters_ShouldApplySortingForUsersList()
	{
		// Arrange
		int pageSize = 20;
		var sortDescriptors = new List<SortDescriptor> { new SortDescriptor { ColumnName = "LastName", SortAscending = true } };
		var users = this.SetupUsers(pageSize);

		var query = new GetUsers() { PageSize = pageSize, SortDescriptors = sortDescriptors };

		// Act
		var response = await this.handler.Handle(query, CancellationToken.None);

		var fakedSortedUsers = users.OrderBy(x => x.LastName).Select(x => x.LastName).ToList();
		var responseSortedUsers = response.Items.Select(x => x.LastName).ToList();

		// Assert
		responseSortedUsers.Should().Equal(fakedSortedUsers);
	}

	/// <summary>
	/// Test that checks if the exception is thrown when the sort parameteres are not valid.
	/// </summary>
	[Fact]
	public async Task Handle_WhenCalledWithInvalidSortParameters_ShouldThrowException()
	{
		// Arrange
		int pageSize = 20;
		var sortDescriptors = new List<SortDescriptor>() { new SortDescriptor { ColumnName = "invalidname", SortAscending = true } };
		var users = this.SetupUsers(pageSize);

		var query = new GetUsers() { PageSize = pageSize, SortDescriptors = sortDescriptors };

		// Act and Assert
		await Assert.ThrowsAsync<NotSupportedException>(async () => await this.handler.Handle(query, CancellationToken.None));
	}

	/// <summary>
	/// Test that checks if the pagination, if specified, is applied for returned values.
	/// </summary>
	[Fact]
	public async Task Handle_WhenPaginationSpecified_ShouldApplyPaginationForUsersList()
	{
		// Arrange
		int pageSize = 20;
		int pageIndex = 3;
		var sortDescriptors = new List<SortDescriptor> { new SortDescriptor { ColumnName = "LastName", SortAscending = true } };
		var users = this.SetupUsers(100);

		var query = new GetUsers() { PageSize = pageSize, PageIndex = pageIndex, SortDescriptors = sortDescriptors };

		// Act
		var response = await this.handler.Handle(query, CancellationToken.None);

		var fakedSortedUsers = users.OrderBy(x => x.LastName).Select(x => x.LastName).Skip(40).Take(20).ToList();
		var responseSortedUsers = response.Items.Select(x => x.LastName).ToList();

		// Assert
		responseSortedUsers.Should().Equal(fakedSortedUsers);
	}

	private List<AppUser> SetupUsers(int numberOfUsers)
	{
		var expectedUsers = new List<AppUser>();

		for (int i = 0; i < numberOfUsers; i++)
		{
			expectedUsers.Add(new AppUser { Email = new Faker().Internet.Email(), FirstName = new Faker().Random.Word(), LastName = new Faker().Random.Word() });
		}

		var content = new StringContent(JsonConvert.SerializeObject(expectedUsers));

		this.keycloakServiceMock.Setup(s => s.GetUsers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.Accepted) { Content = content });

		return expectedUsers;
	}

	private void FakeTokenResponse()
	{
		var token = new Token() { AccessToken = "token" };
		var content = new StringContent(JsonConvert.SerializeObject(token));
		this.keycloakServiceMock.Setup(s => s.GetClientToken(It.IsAny<CancellationToken>())).ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.Accepted) { Content = content });
	}

}