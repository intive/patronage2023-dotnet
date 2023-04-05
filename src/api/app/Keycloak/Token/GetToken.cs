using Intive.Patronage2023.Shared.Abstractions.Commands;

namespace Intive.Patronage2023.Api.Keycloak.Token;

/// <summary>
/// .
/// </summary>
public record GetToken(Guid ClientId) : ICommand;

/// <summary>
/// .
/// </summary>
public class GetTokenHandler : ICommandHandler<GetToken>
{
	/// <summary>
	/// .
	/// </summary>
	/// <param name="command">1.</param>
	/// <param name="cancellationToken">2.</param>
	/// <returns>3.</returns>
	/// <exception cref="NotImplementedException">4.</exception>
	public Task Handle(GetToken command, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}