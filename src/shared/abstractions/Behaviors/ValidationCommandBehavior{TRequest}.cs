using FluentValidation;

using MediatR;

namespace Intive.Patronage2023.Shared.Abstractions.Behaviors;

/// <summary>
/// A pipeline behavior that validates the request using FluentValidation.
/// </summary>
/// <typeparam name="TRequest">The type of the request to be validated.</typeparam>
public sealed class ValidationCommandBehavior<TRequest> : IPipelineBehavior<TRequest, Unit>
	where TRequest : IRequest
{
	private readonly IEnumerable<IValidator<TRequest>> validators;

	/// <summary>
	/// Initializes a new instance of the <see cref="ValidationCommandBehavior{TRequest}"/> class.
	/// </summary>
	/// <param name="validators">The validators to be used for validating the request.</param>
	public ValidationCommandBehavior(IEnumerable<IValidator<TRequest>> validators)
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
	public async Task<Unit> Handle(TRequest request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
	{
		if (!this.validators.Any())
		{
			return await next();
		}

		var context = new ValidationContext<TRequest>(request);

		foreach (var validator in this.validators)
		{
			var result = await validator.ValidateAsync(context, cancellationToken);
			if (!result.IsValid)
			{
				throw new ValidationException(result.Errors);
			}
		}

		return await next();
	}
}