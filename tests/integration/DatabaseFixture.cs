using Docker.DotNet;
using MySql.Data.MySqlClient;


namespace Intive.Patronage2023.Modules.Example.Application.IntegrationTest
{
    public class DatabaseFixture : IDisposable
    {
        public readonly MySqlConnection Connection;

        public DatabaseFixture()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();
            var container = new GenericContainer(dockerClient, "mysql:8.0")
                .WithExposedPorts(3306)
                .WithEnv("MYSQL_ROOT_PASSWORD", "test")
                .WithEnv("MYSQL_DATABASE", "test")
                .Start();

            var port = container.GetMappedPort(3306);
            var connectionString = $"Server=localhost;Port={port};Database=test;Uid=root;Pwd=test;";
            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }

}
