using FluentValidation;

namespace Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;

/// <summary>
/// Example validator class.
/// </summary>
public class CreateExampleValidator : AbstractValidator<CreateExample>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CreateExampleValidator"/> class.
	/// </summary>
	public CreateExampleValidator()
	{
		this.RuleFor(example => example.Id).NotEmpty().NotNull();
		this.RuleFor(example => example.Name).NotEmpty().NotNull();
	}
}