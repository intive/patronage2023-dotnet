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
	/// Method throwing example errors.
	/// </summary>
	/// <exception cref="AppException">Thrown when an application-specific error occurs, such as when a database operation fails.</exception>
	/// <exception cref="FileNotFoundException">Thrown when a file is not found at the specified path.</exception>
	/// <exception cref="UnauthorizedAccessException">Thrown when a user is not authorized to access a resource.</exception>
	/// <exception cref="AuthenticationException">Thrown when user authentication fails.</exception>
	/// <exception cref="Exception">Thrown for any other unhandled exception.</exception>
	/// <remarks>
	/// System.Security.Authentication is just a stub for future implementation of actual authentication method.
	/// </remarks>
	public void ExampleErrors()
	{
		throw new AppException("e.g. \"An exception has been raised that is likely due to a transient failure." +
			"Consider enabling transient error resiliency by adding 'EnableRetryOnFailure' to the 'UseSqlServer' call.\"");

		throw new FileNotFoundException("The page could not be found.");

		throw new UnauthorizedAccessException("Access denied: you are not authorized to access this resource.");

		throw new AuthenticationException("Authentication failed: please provide valid credentials.");

		throw new Exception("An unexpected error has occurred.");
	}
}