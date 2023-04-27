using System.Security.Authentication;
using Intive.Patronage2023.Modules.Example.Api.Services;
using Intive.Patronage2023.Modules.Example.Application.Example;
using Xunit;
using FluentAssertions;

namespace Intive.Patronage2023.Example.Api.Tests;

/// <summary>
/// Tests that checks correct behaviour of errors thrown by "ExampleErrorService".
/// </summary>
public class ExampleErrorServiceTests
{
	private readonly ExampleErrorService errorService;

	/// <summary>
	/// Constructor for the ExampleErrorServiceTests class.
	/// </summary>
	public ExampleErrorServiceTests()
	{
		this.errorService = new ExampleErrorService();
	}

	/// <summary>
	/// Test that verifies if 'AppException' is correctly thrown.
	/// </summary>
	[Fact]
	public void ThrowAppException_ShouldThrowAppException()
	{
		// Act & Assert
		FluentActions.Invoking(this.errorService.ThrowAppException)
			.Should()
			.Throw<AppException>()
			.WithMessage("e.g. \"An exception has been raised that is likely due to a transient failure." +
		"Consider enabling transient error resiliency by adding 'EnableRetryOnFailure' to the 'UseSqlServer' call.\"");
	}

	/// <summary>
	/// Test that verifies if 'UnauthorizedAccessException' is correctly thrown.
	/// </summary>
	[Fact]
	public void ThrowUnauthorizedAccessException_ShouldThrowUnauthorizedAccessException()
	{
		// Act & Assert
		FluentActions.Invoking(this.errorService.ThrowUnauthorizedAccessException)
			.Should()
			.Throw<UnauthorizedAccessException>()
			.WithMessage("Access denied: you are not authorized to access this resource.");
	}

	/// <summary>
	/// Test that verifies if 'AuthenticationException' is correctly thrown.
	/// </summary>
	[Fact]
	public void ThrowAuthenticationException_ShouldThrowAuthenticationException()
	{
		// Act & Assert
		FluentActions.Invoking(this.errorService.ThrowAuthenticationException)
			.Should()
			.Throw<AuthenticationException>()
			.WithMessage("Authentication failed: please provide valid credentials.");
	}

	/// <summary>
	/// Test that verifies if 'Exception' is correctly thrown.
	/// </summary>
	[Fact]
	public void ThrowException_ShouldThrowException()
	{
		// Act & Assert
		FluentActions.Invoking(this.errorService.ThrowException)
			.Should()
			.Throw<Exception>()
			.WithMessage("An unexpected error has occurred.");
	}
}
