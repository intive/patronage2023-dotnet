using FluentValidation;
using MediatR;

namespace Intive.Patronage2023.Shared.Abstractions.Behaviors;

/// <summary>
/// A pipeline behavior that validates the request using FluentValidation.
/// </summary>
/// <typeparam name="TRequest">The type of the request to be validated.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the request.</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> validators;

	/// <summary>
	/// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
	/// </summary>
	/// <param name="validators">The validators to be used for validating the request.</param>
	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		this.validators = validators;
	}

	/// <summary>
	/// Handles the request by validating it using the specified validators.
	/// </summary>
	/// <param name="request">The request to be validated.</param>
	/// <param name="next">The delegate to be called for the next behavior in the pipeline.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The response returned by the request.</returns>
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (!this.validators.Any())
		{
			return await next();
		}

		var context = new ValidationContext<TRequest>(request);

		var errorsDictionary = this.validators
			.Select(x => x.Validate(context))
			.SelectMany(x => x.Errors)
			.Where(x => x != null)
			.GroupBy(
				x => x.PropertyName,
				x => x.ErrorMessage,
				(propertyName, errorMessages) => new
				{
					Key = propertyName,
					Values = errorMessages.Distinct().ToArray(),
				})
			.ToDictionary(x => x.Key, x => x.Values);
		if (errorsDictionary.Any())
		{
			var errorsDictionaryValues = new List<string>();
			foreach (var kvp in errorsDictionary)
			{
				errorsDictionaryValues.AddRange(kvp.Value);
			}

			throw new ValidationException(string.Join(" & ", errorsDictionaryValues));
		}

		return await next();
	}
}