using System.Text.Json.Serialization;
using Hangfire.Dashboard;
using Intive.Patronage2023.Api.Configuration;
using Intive.Patronage2023.Api.Errors;
using Intive.Patronage2023.Modules.Budget.Api;
using Intive.Patronage2023.Modules.Example.Api;
using Intive.Patronage2023.Modules.User.Api;
using Intive.Patronage2023.Modules.User.Api.Configuration;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions.Behaviors;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.Commands.CommandBus;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;
using Intive.Patronage2023.Shared.Infrastructure.Queries.QueryBus;
using Keycloak.AuthServices.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

string corsPolicyName = "CorsPolicy";

builder.Services.AddCors(builder.Configuration, corsPolicyName);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddTelemetry();
builder.Services.AddHttpLogging(logging =>
{
	logging.LoggingFields = HttpLoggingFields.All;
	logging.RequestBodyLogLimit = 4096;
	logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddSharedModule();
builder.Services.AddExampleModule(builder.Configuration);
builder.Services.AddBudgetModule(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddUserModule();

builder.Services.Configure<ApiKeycloakSettings>(builder.Configuration.GetSection("Keycloak"));
builder.Services.AddHttpClient();
builder.Services.AddScoped<IKeycloakService, KeycloakService>();
builder.Services.AddImportExportModule(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
	cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddControllers(options =>
	options.Filters.Add(new AuthorizeFilter(
		new AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser()
			.Build())))
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
			});

builder.Services.AddFromAssemblies(typeof(IDomainEventHandler<>), AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFromAssemblies(typeof(IEventDispatcher<>), AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFromAssemblies(typeof(ICommandHandler<>), AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFromAssemblies(typeof(IQueryHandler<,>), AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICommandBus, CommandBus>();
builder.Services.AddScoped<IQueryBus, QueryBus>();
builder.Services.AddFromAssemblies(typeof(IRepository<,>), AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddKeycloakAuthentication(builder.Configuration, configureOptions =>
{
	// turning off issuer validation and https
	configureOptions.RequireHttpsMetadata = false;
	configureOptions.TokenValidationParameters.ValidateIssuer = false;
});
builder.Services.AddAuthorization();

builder.Services.AddSwagger();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient(
	typeof(IPipelineBehavior<,>),
	typeof(ValidationQueryBehavior<,>));

builder.Services.AddCommandBehavior(typeof(ValidationCommandBehavior<>), AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHangfireService(builder.Configuration);

var app = builder.Build();

app.UseCors(corsPolicyName);

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

app.UseExampleModule();
app.UseBudgetModule();
app.UseUserModule();

app.UseAuthentication();
app.UseAuthorization();

var scope = app.Services.CreateScope();
var authorizationFilter = scope.ServiceProvider.GetRequiredService<IDashboardAuthorizationFilter>();
app.UseHangfireService(authorizationFilter);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();