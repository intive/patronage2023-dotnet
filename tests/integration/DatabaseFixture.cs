using Docker.DotNet;
using Microsoft.Data.SqlClient;

namespace Intive.Patronage2023.Modules.Example.Application.IntegrationTest
{
	public class DatabaseFixture : IDisposable
	{
		public readonly SqlConnection Connection;

		public DatabaseFixture()
		{
			var dockerClient = new DockerClientConfiguration().CreateClient();
			var container = new GenericContainer(dockerClient, "mcr.microsoft.com/mssql/server:2019-latest")
				.WithExposedPorts(1433)
				.WithEnv("ACCEPT_EULA")
				.WithEnv("PASSWORD=")
				.WaitForPort("1433/tcp", 30000);
			container.Start();
			var hostPort = container.GetMappedPort(1433);

			var connectionString = new SqlConnectionStringBuilder()
			{
				DataSource = $"localhost,{hostPort}",
				InitialCatalog = "DB_name",
				UserID = "",
				Password = ""
			}.ConnectionString;

			Connection = new SqlConnection(connectionString);
			Connection.Open();
		}

		public void Dispose()
		{
			Connection.Dispose();
		}
	}
}
