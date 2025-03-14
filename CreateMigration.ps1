# Prompt the user for the migration name
$MigrationName = Read-Host "Enter the migration name"

# Define the DbContext
$DbContext = "DeveSpotnet.Db.DeveSpotnetDbContext"  # Use the fully qualified name
$StartupProject = "./DeveSpotnet"  # Point to the project containing Program.cs

# Create migrations for both providers
dotnet ef migrations add $MigrationName --project "./Migrations/DeveSpotnet.Migrations.Postgres" --startup-project $StartupProject --context $DbContext -- --DatabaseProvider Postgres
dotnet ef migrations add $MigrationName --project "./Migrations/DeveSpotnet.Migrations.Sqlite" --startup-project $StartupProject --context $DbContext -- --DatabaseProvider Sqlite