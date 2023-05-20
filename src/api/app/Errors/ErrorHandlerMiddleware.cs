using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;
using Intive.Patronage2023.Shared.Infrastructure.Domain;

namespace Intive.Patronage2023.Api.Errors;

/// <summary>
/// Middleware to handle exceptions and return appropriate HTTP responses.
/// </summary>
public class ErrorHandlerMiddleware
{
	private readonly RequestDelegate next;

	/// <summary>
	/// Initializes a new instance of the <see cref="ErrorHandlerMiddleware"/> class.
	/// </summary>
	/// <param name="next">The next delegate in the request pipeline.</param>
	public ErrorHandlerMiddleware(RequestDelegate next)
	{
		this.next = next;
	}

	/// <summary>
	/// Invokes the middleware.
	/// </summary>
	/// <param name="context">The HTTP context.</param>
	/// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
	public async Task Invoke(HttpContext context)
	{
		try
		{
			await this.next(context);
		}
		catch (AppException ex)
		{
			await HandleErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
		}
		catch (AuthenticationException ex)
		{
			await HandleErrorAsync(context, HttpStatusCode.Unauthorized, ex.Message);
		}
		catch (UnauthorizedAccessException ex)
		{
			await HandleErrorAsync(context, HttpStatusCode.Forbidden, ex.Message);
		}
		catch (FluentValidation.ValidationException ex)
		{
			await HandleErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
		}
		catch (BusinessRuleValidationException ex)
		{
			await HandleErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
		}
		catch (Exception ex)
		{
			await HandleErrorAsync(context, HttpStatusCode.InternalServerError, ex.Message);
		}
	}

	/// <summary>
	/// Handles an error by setting the response status code, serializes error message and writes it to response body.
	/// </summary>
	/// <param name="context">The HTTP context.</param>
	/// <param name="statusCode">The HTTP status code.</param>
	/// <param name="message">The error message.</param>
	/// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
	private static async Task HandleErrorAsync(HttpContext context, HttpStatusCode statusCode, string message)
	{
		var response = context.Response;
		response.ContentType = "application/json";
		response.StatusCode = (int)statusCode;

		string result = JsonSerializer.Serialize(new { message });
		await response.WriteAsync(result);
	}
}