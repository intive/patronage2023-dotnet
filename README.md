# -patronage2023-dotnet

## Setting up local development environment

### Database connection

The default connection string is called `AppDb` and is stored in `appsetings.json` file. Check the file to confirm that the connection string to your local database is correct. The context is configured in `ExampleModule.cs (Modules.Example.Api project)` with the connection string being read from configuration.

### Migrations

In order to execute EF Core commands from .NET CLI install EF Core Tools.

To create a new migration you can use .NET CLI and execute the following command: 

```
dotnet ef migrations add <NAME> --project <PathToProjectWhereIsDbContext> --startup-project <PathToMainApiProject> --context <ContextName> --output-dir <PathToMigrationsDir> --configuration Debug --no-build
```
 
Migration script example: 

```
dotnet ef migrations add ExampleMigration --project .\src\modules\example\infrastructure --startup-project .\src\api\app --context ExampleDbContext .\Migrations --configuration Debug --no-build
```

In order to create the database, apply database migrations by executing: 

```
dotnet ef database update --startup-project <PathToMainApiProject>
```

Optionally, you can add the specific name of migration that you want the database to be updated to: 

```
dotnet ef database update -MigrationName
```