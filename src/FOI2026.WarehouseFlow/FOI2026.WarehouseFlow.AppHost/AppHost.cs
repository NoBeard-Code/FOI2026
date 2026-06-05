var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameter("sql-password", secret: true); // should be stored in user secrets (Parameters:sql-password)

var sqlServer = builder.AddSqlServer("foi2026-warehouseflow-sql-server", sqlPassword)
    .WithDataVolume("foi2026-warehouseflow-sql-volume")
    .WithLifetime(ContainerLifetime.Persistent);

var sqlDatabase = sqlServer.AddDatabase("foi2026-warehouseflow-sql-db");

builder.AddProject<Projects.FOI2026_WarehouseFlow_Web>("foi2026-warehouseflow-web")
    .WithReference(sqlDatabase)
    .WaitFor(sqlDatabase);

builder.Build().Run();
