using Intive.Patronage2023.Modules.Example.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;

/// <summary>
/// Create Example command.
/// </summary>
/// <param name="Id">Example identifier.</param>
/// <param name="Name">Example name.</param>
public record CreateExample(Guid Id, string Name) : ICommand;

/// <summary>
/// Create example.
/// </summary>
public class HandleCreateExample : ICommandHandler<CreateExample>
{
	private readonly IRepository<ExampleAggregate, ExampleId> exampleRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleCreateExample"/> class.
	/// </summary>
	/// <param name="exampleRepository">Repository that manages example aggregate root.</param>
	public HandleCreateExample(IRepository<ExampleAggregate, ExampleId> exampleRepository)
	{
		this.exampleRepository = exampleRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(CreateExample command, CancellationToken cancellationToken)
	{
		var example = ExampleAggregate.Create(command.Id, command.Name);

		await this.exampleRepository.Persist(example);
	}
}