using Task3.Abstractions;
using Task3.DataAccess.Data;
using Task3.DataAccess.Repositories;
using Task3.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(typeof(IUserRepository),
    (x) => new InMemoryUserRepository(FakeDataFactory.Users));

builder.Services.AddSingleton(typeof(ICoinRepository),
    (x) => new InMemoryCoinRepository());

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<BillingService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();