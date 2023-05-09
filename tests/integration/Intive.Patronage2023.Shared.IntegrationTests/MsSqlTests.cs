using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Intive.Patronage2023.Shared.IntegrationTests;

/// <summary>
/// Integration tests for the MS SQL database using Testcontainers.
/// </summary>
public class MsSqlTests : IAsyncLifetime
{
	///<summary>
	/// The name of the database used for MS SQL tests.
	///</summary>
	public const string Database = "patronage2023";

	///<summary>
	/// The username used for MS SQL connection string.
	///</summary>
	public const string Username = "sa";

	///<summary>
	/// The password used for MS SQL connection string.
	///</summary>
	public const string Password = "yourStrong(!)Password";

	///<summary>
	/// The port number used for MS SQL tests.
	///</summary>
	public const ushort MsSqlPort = 1433;

	///<summary>
	/// The port number used for mapping to the container port.
	///</summary>
	public const ushort MappedPort = 5000;

	///<summary>
	/// The container used for running the MS SQL server for testing.
	///</summary>
	public readonly IContainer mssqlContainer = new ContainerBuilder()
		.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
		.WithPortBinding(MappedPort.ToString(), MsSqlPort.ToString())
		.WithEnvironment("ACCEPT_EULA", "Y")
		.WithEnvironment("SQLCMDUSER", Username)
		.WithEnvironment("SQLCMDPASSWORD", Password)
		.WithEnvironment("MSSQL_SA_PASSWORD", Password)
		.WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("/opt/mssql-tools/bin/sqlcmd", "-Q", "SELECT 1;"))
		.Build();

	/// <summary>
	/// Initializes the MsSql container for integration tests.
	/// </summary>
	public Task InitializeAsync()
	{
		return this.mssqlContainer.StartAsync();
	}

	/// <summary>
	/// Disposes the MsSql container after integration tests have been executed.
	/// </summary>
	public Task DisposeAsync()
	{
		return this.mssqlContainer.DisposeAsync().AsTask();
	}
}