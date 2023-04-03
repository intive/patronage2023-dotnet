using Intive.Patronage2023.Shared.Abstractions.Commands;

namespace Intive.Patronage2023.Modules.Example.Application.User.CreatingUser;

/// <summary>
/// Create user command.
/// </summary>
/// <param name="Id">Id.</param>
/// <param name="FirstName">FirstName.</param>
/// <param name="LastName">LastName.</param>
/// <param name="Password">Password.</param>
/// <param name="Email">Email.</param>
public record CreateUser(Guid Id, string FirstName, string LastName, string Password, string Email) : ICommand;