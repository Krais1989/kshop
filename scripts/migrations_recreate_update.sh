rm -rf Migrations
dotnet ef migrations add InitialCommit
dotnet ef database drop
dotnet ef database update