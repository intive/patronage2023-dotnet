namespace Intive.Patronage2023.Modules.Budget.Application.IntegrationTests;

/// <summary>
/// Dummy class for collection definition.
/// </summary>
[CollectionDefinition("Database collection", DisableParallelization = false)]
public class DatabaseDefinitionTestFixtureCollection : ICollectionFixture<MsSqlTests>
{
}