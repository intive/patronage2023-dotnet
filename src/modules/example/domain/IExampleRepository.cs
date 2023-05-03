using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Example.Domain;

/// <summary>
/// Interface of repository for ExampleAggregate.
/// </summary>
public interface IExampleRepository : IRepository<ExampleAggregate, Guid>
{
}