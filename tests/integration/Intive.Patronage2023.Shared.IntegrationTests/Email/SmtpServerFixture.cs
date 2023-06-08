using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Intive.Patronage2023.Shared.IntegrationTests.Email;

/// <summary>
/// Email service integration tests.
/// </summary>
public class SmtpServerFixture : IAsyncLifetime
{
	/// <summary>
	/// Mapped default port.
	/// </summary>
	public const int Port = 465;

	/// <summary>
	/// Default port of smtp server.
	/// </summary>
	private const int Smtp4devPort = 25;

	///<summary>
	/// The container used for running the smtp4dev for testing.
	///</summary>
	public readonly IContainer smtp4devcontainer = new ContainerBuilder()
		.WithImage("rnwood/smtp4dev:latest")
		.WithPortBinding(Port, Smtp4devPort)
		.WithWaitStrategy(Wait.ForUnixContainer())
		.Build();

	/// <summary>
	/// Initializes the smtp4devcontainer container for integration tests.
	/// </summary>
	public Task InitializeAsync()
	{
		return this.smtp4devcontainer.StartAsync();
	}

	/// <summary>
	/// Disposes the MsSql container after integration tests have been executed.
	/// </summary>
	public Task DisposeAsync()
	{
		return this.smtp4devcontainer.DisposeAsync().AsTask();
	}
}