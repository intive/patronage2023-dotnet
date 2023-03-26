using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;

namespace Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;

/// <summary>
/// Create Example command.
/// </summary>
/// <param name="Id">Example identifier.</param>
/// <param name="Name">Example name.</param>
public record CreateExample(Guid Id, string Name);

/// <summary>
/// Create example.
/// </summary>
public class HandleCreateExample : ICommandHandler<CreateExample>
{
	private readonly IExampleRepository exampleRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleCreateExample"/> class.
	/// </summary>
	/// <param name="exampleRepository">Repository that manages example aggregate root.</param>
	public HandleCreateExample(IExampleRepository exampleRepository)
	{
		this.exampleRepository = exampleRepository;
	}

	/// <inheritdoc/>
	public Task Handle(CreateExample command)
	{
		var example = ExampleAggregate.Create(command.Id, command.Name);

		this.exampleRepository.Persist(example);
		return Task.CompletedTask;
	}
}