# Prompt the user for the migration name
$MigrationName = Read-Host "Enter the migration name"

# Define the DbContext
$DbContext = "DeveSpotnetDbContext"

dotnet ef migrations add $MigrationName --project "./Migrations/DeveSpotnet.Migrations.Postgres" --context $DbContext -- --provider PostgreSQL
dotnet ef migrations add $MigrationName --project "./Migrations/DeveSpotnet.Migrations.Sqlite" --context $DbContext -- --provider Sqlite
