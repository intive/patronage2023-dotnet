using Intive.Patronage2023.Shared.Abstractions.Commands;

namespace Intive.Patronage2023.Modules.Example.Api.User.CreatingUser;

/// <summary>
/// Create user command.
/// </summary>
/// <param name="Id">User identifier.</param>
/// <param name="FirstName">FirstName which user provides.</param>
/// <param name="LastName">LastName which user provides.</param>
/// <param name="Password">Password which user provides.</param>
/// <param name="Email">Email which user provides.</param>
public record CreateUser(Guid Id, string FirstName, string LastName, string Password, string Email) : ICommand;