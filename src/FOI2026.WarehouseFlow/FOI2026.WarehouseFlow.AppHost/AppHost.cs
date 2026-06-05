var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.FOI2026_WarehouseFlow_Web>("foi2026-warehouseflow-web");

builder.Build().Run();
