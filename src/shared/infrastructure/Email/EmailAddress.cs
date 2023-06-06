namespace Intive.Patronage2023.Shared.Infrastructure.Email;

/// <summary>
/// Email  address details.
/// </summary>
/// <param name="Name">Email adress name to identify.</param>
/// <param name="Address">Email address.</param>
public record EmailAddress(string Name, string Address);