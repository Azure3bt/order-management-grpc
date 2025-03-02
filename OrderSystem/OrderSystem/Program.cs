using Microsoft.EntityFrameworkCore;
using OrderSystem.Persistence;
using OrderSystem.Services;
using ShcoolSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddDbContext<OrderDbContext>(
    options => options.UseInMemoryDatabase(nameof(OrderSystem))
);

var app = builder.Build();

//create database first load!
using var scop = app.Services.CreateScope();
var db = scop.ServiceProvider.GetRequiredService<OrderDbContext>();
await db.Database.EnsureCreatedAsync();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<OrderService>();
app.MapGrpcService<ShcoolService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
