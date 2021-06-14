rm -rf Migrations
dotnet ef migrations add InitialCommit
dotnet ef database drop -f
dotnet ef database update