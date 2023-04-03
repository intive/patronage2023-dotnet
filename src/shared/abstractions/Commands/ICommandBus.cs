namespace Intive.Patronage2023.Shared.Abstractions.Commands;

/// <summary>
/// Interface to send commands.
/// </summary>
public interface ICommandBus
{
    /// <summary>
    /// Sends command to the bus.
    /// </summary>
    /// <typeparam name="TCommand">Type of command.</typeparam>
    /// <param name="command">Command to send.</param>
    /// <returns>Task that represents asynchronous operation.</returns>
    Task Send<TCommand>(TCommand command)
        where TCommand : ICommand;
}