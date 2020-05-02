# How to update DB with EF core
- Go to visual studio, open `Package Manager Console`
- Set default project to `Driver`
- Update table schema in migration folder
    - `Add-Migration **MigrationName** -Project Model`
- Apply to database
    - `Update-Database -Project Model`

[Reference](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)