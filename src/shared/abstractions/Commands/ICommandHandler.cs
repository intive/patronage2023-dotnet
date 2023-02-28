namespace Intive.Patronage2023.Shared.Abstractions.Commands;

/// <summary>
/// Interface of command handler.
/// </summary>
/// <typeparam name="T">Type of command to handle.</typeparam>
public interface ICommandHandler<T>
{
	/// <summary>
	/// Handles command.
	/// </summary>
	/// <param name="command">Command to handle.</param>
	/// <returns>Task representing asynchronous operation.</returns>
	Task Handle(T command);
}