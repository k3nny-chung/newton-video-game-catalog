# Getting started
Update the connection string in the appsettings file to the connection string for your SQL Server (https://github.com/k3nny-chung/newton-video-game-catalog/blob/master/VideoGamesCatalog.Server/appsettings.Development.json#L9)

Run the EF migration tool from the solution folder to create the database objects and seed the database:

`dotnet ef database update --project VideoGamesCatalog.Core --startup-project VideoGamesCatalog.Server`
