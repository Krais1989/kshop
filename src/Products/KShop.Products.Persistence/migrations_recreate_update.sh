rm -rf Migrations
dotnet ef database drop -f
dotnet ef migrations add InitialCommit
dotnet ef database update