using System;
using Intive.Patronage2023.Modules.Example.Api;
using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Intive.Patronage2023.Modules.Example.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExampleModule();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDb")));

builder.Services.AddScoped<IExampleRepository, ExampleRepository>();

var app = builder.Build();

app.UseExampleModule();

app.Run();