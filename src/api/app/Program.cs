using System.Reflection;
using Intive.Patronage2023.Modules.Example.Api;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExampleModule();

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "Intive Patronage2023 Some Title Api",
		Description = "An ASP.NET Core Web API for managing bills and more",

		// Idk if needed and what to put in TOS, Contact and License
		// TermsOfService = new Uri("https://example.com/terms"),
		// Contact = new OpenApiContact
		// {
		// 	Name = "Example Contact",
		// 	Url = new Uri("https://example.com/contact"),
		// },
		// License = new OpenApiLicense
		// {
		// 	Name = "Example License",
		// 	Url = new Uri("https://example.com/license"),
		// },
	});

	string xmlExampleApiDocFilename = "Intive.Patronage2023.Modules.Example.Api.xml";

	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlExampleApiDocFilename));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseExampleModule();

app.Run();