using DotNet.Testcontainers.Builders;
using FluentAssertions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IContainer = DotNet.Testcontainers.Containers.IContainer;
using System.Threading.Tasks;

namespace Intive.Patronage2023.Modules.Example.Application.IntegrationTests;

/// <summary>
/// Integration tests for the MS SQL database using Testcontainers.
/// </summary>
public class MsSqlTests : IAsyncLifetime
{
	public const string Database = "patronage2023";
	public const string Username = "sa";
	public const string Password = "yourStrong(!)Password";
	public const ushort MsSqlPort = 1433;
	public const ushort MappedPort = 5000;

	public readonly IContainer? _mssqlContainer = new ContainerBuilder()
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
		return this._mssqlContainer?.StartAsync() ?? Task.CompletedTask;
	}

	/// <summary>
	/// Disposes the MsSql container after integration tests have been executed.
	/// </summary>
	public Task DisposeAsync()
	{
		return this._mssqlContainer?.DisposeAsync().AsTask() ?? Task.CompletedTask;
	}
}
