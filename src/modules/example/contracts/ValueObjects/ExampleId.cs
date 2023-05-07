using Intive.Patronage2023.Shared.Infrastructure;

namespace Intive.Patronage2023.Modules.Example.Contracts.ValueObjects;

/// <summary>
/// record which represents Example Id.
/// </summary>
public record struct ExampleId(Guid Value) : IEntityId<Guid>;