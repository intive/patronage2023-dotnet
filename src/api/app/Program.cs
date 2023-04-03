// using Intive.Patronage2023.Modules.Example.Api;
using Intive.Patronage2023.Modules.Example.Api;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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

builder.Services.AddFromAssemblies(typeof(IDomainEventHandler<>));
builder.Services.AddFromAssemblies(typeof(IEventDispatcher<>));
builder.Services.AddFromAssemblies(typeof(ICommandHandler<>));
builder.Services.AddFromAssemblies(typeof(IQueryHandler<,>));

using var loggerFactory = LoggerFactory.Create(builder =>
{
	builder.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Disabled);
});

var logger = loggerFactory.CreateLogger<Program>();

builder.Logging.AddApplicationInsights(
		configureTelemetryConfiguration: (config) =>
			config.ConnectionString = builder.Configuration.GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING"),
		configureApplicationInsightsLoggerOptions: (options) => { });

builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("your-category", Microsoft.Extensions.Logging.LogLevel.Trace);

using var channel = new InMemoryChannel();

try
{
	IServiceCollection services = new ServiceCollection();
	services.Configure<TelemetryConfiguration>(config => config.TelemetryChannel = channel);
	services.AddLogging(builder =>
	{
		// Only Application Insights is registered as a logger provider
		builder.AddApplicationInsights(
			configureTelemetryConfiguration: (config) => config.ConnectionString = "<OurConnectionString>",
			configureApplicationInsightsLoggerOptions: (options) => { });
	});

	IServiceProvider serviceProvider = services.BuildServiceProvider();
	ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();

	logger.LogInformation("Logger is working...");
}
finally
{
	// Explicitly call Flush() followed by Delay, as required in console apps.
	// This ensures that even if the application terminates, telemetry is sent to the back end.
	channel.Flush();

	await Task.Delay(TimeSpan.FromMilliseconds(1000));
}

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/Test", async context =>
{
	logger.LogInformation("Testing logging in Program.cs");
	await context.Response.WriteAsync("Testing");
});

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpLogging();
app.UseHttpsRedirection();

app.MapControllers();

app.UseExampleModule();
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();