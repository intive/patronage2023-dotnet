using Intive.Patronage2023.Modules.Example.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExampleModule();

var app = builder.Build();

app.UseExampleModule();

app.Run();