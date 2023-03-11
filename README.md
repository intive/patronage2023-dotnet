# -patronage2023-dotnet

## Setting up local development environment:

1. Check the file appsetings.json to confirm that the connection string to your local database is correct. Modify it if necessary. 
2. To apply migrations in Visual Studio go to Package Manager Console, set the project 'Intive.Patronage2023.Api' as default and run command 'Update-Database'.  
3. To create a new migration run 'Add-Migration [Migration Name]'.
