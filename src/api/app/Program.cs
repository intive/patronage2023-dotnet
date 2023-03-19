using Intive.Patronage2023.Modules.Example.Api;
using Intive.Patronage2023.Shared.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string corsPolicyName = "CorsPolicy";
ConfigureCorsPolicy(builder.Services, builder.Configuration, corsPolicyName);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddExampleModule(builder.Configuration);
builder.Services.AddHttpLogging(logging =>
{
	logging.LoggingFields = HttpLoggingFields.All;
	logging.RequestBodyLogLimit = 4096;
	logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddSharedModule();

builder.Services.AddMediatR(cfg =>
{
	cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "Intive Patronage2023 Some Title Api",
		Description = "An ASP.NET Core Web API for managing bills and more",
	});

	// Searching for all files with ".Api.xml" suffix, which should be api docs,
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

app.UseCors(corsPolicyName);

app.UseHttpLogging();
app.UseHttpsRedirection();

app.MapControllers();

app.UseExampleModule();

app.Run();

static void ConfigureCorsPolicy(IServiceCollection services, ConfigurationManager configuration, string corsPolicyName)
{
	string? allowedOrigins = configuration.GetValue<string>("AllowedOrigins");
	if (!string.IsNullOrEmpty(allowedOrigins))
	{
		services.AddCors(options =>
		{
			options.AddPolicy(
			name: corsPolicyName,
			policy =>
			{
				string[] origins = allowedOrigins.Split(";");
				policy.AllowAnyMethod()
					  .AllowAnyHeader()
					  .WithOrigins(origins);
			});
		});
	}
}
