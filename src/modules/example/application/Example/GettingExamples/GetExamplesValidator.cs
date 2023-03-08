using FluentValidation;

namespace Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples
{
	/// <summary>
	/// GetExamplesValidator class.
	/// </summary>
	public class GetExamplesValidator : AbstractValidator<GetExamples>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GetExamplesValidator"/> class.
		/// </summary>
		public GetExamplesValidator()
		{
			this.RuleFor(x => x).NotEmpty().NotNull().WithMessage("Examples cannot be empty.");
		}
	}
}
