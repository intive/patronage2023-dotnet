using FluentValidation.Results;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// Provides extension methods for working with validation results.
/// </summary>
public static class ValidationResultExtensions
{
	/// <summary>
	/// Adds validation errors to the list of errors.
	/// </summary>
	/// <param name="errors">The list of errors.</param>
	/// <param name="validationResult">The validation result to extract errors from.</param>
	public static void AddErrors(this List<string> errors, ValidationResult validationResult)
	{
		errors.AddRange(validationResult.Errors.Select(result => $"{result.ErrorMessage}"));
	}
}