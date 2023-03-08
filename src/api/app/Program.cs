using System.Reflection;
using Intive.Patronage2023.Modules.Example.Api;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExampleModule();

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "Intive Patronage2023 Some Title Api",
		Description = "An ASP.NET Core Web API for managing bills and more",
	});

	// Searching for all files with "Api.xml" suffix, which should be api docs,
	// in build directory and attach them to swagger
	var xmlFiles = Directory.GetFiles(
		AppContext.BaseDirectory,
		"*.Api.xml",
		SearchOption.TopDirectoryOnly).ToList();
	xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseExampleModule();

app.Run();