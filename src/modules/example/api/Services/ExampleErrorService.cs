using System.Security.Authentication;
using Intive.Patronage2023.Modules.Example.Application.Example;

namespace Intive.Patronage2023.Modules.Example.Api.Services;

/// <summary>
/// Example service that shows how exceptions handled by global error handler
/// should be thrown.
/// </summary>
public class ExampleErrorService
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleErrorService"/> class.
	/// </summary>
	public ExampleErrorService()
	{
	}

	/// <summary>
	/// Method throwing AppExceptionError.
	/// </summary>
	/// <exception cref="AppException">Thrown when an application-specific error occurs, such as when a database operation fails.</exception>
	public void ThrowAppException()
	{
		throw new AppException("e.g. \"An exception has been raised that is likely due to a transient failure." +
			"Consider enabling transient error resiliency by adding 'EnableRetryOnFailure' to the 'UseSqlServer' call.\"");
	}

	/// <summary>
	/// Method throwing FileNotFoundException.
	/// </summary>
	/// <exception cref="FileNotFoundException">Thrown when a file is not found at the specified path.</exception>
	public void ThrowFileNotFoundException()
	{
		throw new FileNotFoundException("The page could not be found.");
	}

	/// <summary>
	/// Method throwing UnauthorizedAccessException.
	/// </summary>
	/// <exception cref="UnauthorizedAccessException">
	/// Thrown when a user is not authorized to access a resource.
	/// </exception>
	public void ThrowUnauthorizedAccessException()
	{
		throw new UnauthorizedAccessException("Access denied: you are not authorized to access this resource.");
	}

	/// <summary>
	/// Method throwing AuthenticationException.
	/// </summary>
	/// <exception cref="AuthenticationException">
	/// Thrown when user authentication fails.
	/// </exception>
	public void ThrowAuthenticationException()
	{
		throw new AuthenticationException("Authentication failed: please provide valid credentials.");
	}

	/// <summary>
	/// Method throwing Exception.
	/// </summary>
	/// <exception cref="Exception">
	/// Thrown for any other unhandled exception.
	/// </exception>
	public void ThrowException()
	{
		throw new Exception("An unexpected error has occurred.");
	}
}