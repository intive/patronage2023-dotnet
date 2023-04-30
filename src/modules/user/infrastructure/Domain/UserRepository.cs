using System.Text.Json;

using Intive.Patronage2023.Modules.User.Domain;
using Intive.Patronage2023.Modules.User.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.User.Infrastructure.Domain;

/// <summary>
/// User repository.
/// </summary>
public class UserRepository : IUserRepository
{
}