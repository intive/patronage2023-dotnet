using Intive.Patronage2023.Shared.Abstractions.Commands;

namespace Intive.Patronage2023.Modules.Example.Application.User.CreatingUser;

/// <summary>
/// Create user command.
/// </summary>
/// <param name="Id">Id.</param>
/// <param name="Username">Username.</param>
/// <param name="Password">Password.</param>
/// <param name="Email">Email.</param>
public record CreateUser(Guid Id, string Username, string Password, string Email) : ICommand;