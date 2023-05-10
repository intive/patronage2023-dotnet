using Intive.Patronage2023.Shared.Infrastructure;

namespace Intive.Patronage2023.Modules.User.Contracts.ValueObjects;

/// <summary>
/// record which represents User Id.
/// </summary>
public record struct UserId(Guid Value) : IEntityId<Guid>;