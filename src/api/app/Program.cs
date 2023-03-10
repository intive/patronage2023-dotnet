using Intive.Patronage2023.Modules.Example.Api;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExampleModule();
builder.Services.AddHttpLogging(logging =>
{
	logging.LoggingFields = HttpLoggingFields.All;
	logging.RequestBodyLogLimit = 4096;
	logging.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

app.UseExampleModule();
app.UseHttpLogging();
app.Run();