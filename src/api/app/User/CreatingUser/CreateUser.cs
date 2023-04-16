using Intive.Patronage2023.Shared.Abstractions.Queries;

namespace Intive.Patronage2023.Api.User.CreatingUser;

/// <summary>
/// Create user command.
/// </summary>
/// <param name="Avatar">Avatar which user chooses.</param>
/// <param name="FirstName">First name which user provides.</param>
/// <param name="LastName">Last name which user provides.</param>
/// <param name="Password">Password which user provides.</param>
/// <param name="Email">Email which user provides.</param>
public record CreateUser(string Avatar, string FirstName, string LastName, string Password, string Email) : IQuery<HttpResponseMessage>;