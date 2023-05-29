using Microsoft.Extensions.Logging;

namespace Intive.Patronage2023.Shared.Infrastructure;

/// <summary>
/// Class contains methods that send log messages.
/// </summary>
public abstract class SendLogMessage
{
	private readonly ILogger<SendLogMessage> logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="SendLogMessage"/> class.
	/// </summary>
	/// <param name="logger">Logger.</param>
	protected SendLogMessage(ILogger<SendLogMessage> logger)
	{
		this.logger = logger;
	}

	/// <summary>
	/// Method that sends log information message.
	/// </summary>
	/// <param name="message">Message to Log.</param>
	public void LogInformation(string message)
	{
		this.logger.LogInformation(message);
	}
}