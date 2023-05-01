using System.Text.Json.Serialization;
using Intive.Patronage2023.Api.Configuration;
using Intive.Patronage2023.Api.Keycloak;
using Intive.Patronage2023.Api.User;
using Intive.Patronage2023.Modules.Budget.Api;
using Intive.Patronage2023.Modules.Example.Api;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.Commands.CommandBus;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;
using Intive.Patronage2023.Shared.Infrastructure.Queries.QueryBus;

using Keycloak.AuthServices.Authentication;

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
builder.Services.AddUserModule(builder.Configuration);

builder.Services.AddBudgetModule(builder.Configuration);
builder.Services.Configure<ApiKeycloakSettings>(builder.Configuration.GetSection("Keycloak"));
builder.Services.AddHttpClient();
builder.Services.AddScoped<KeycloakService>();

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
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddFromAssemblies(typeof(IDomainEventHandler<>), AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFromAssemblies(typeof(IEventDispatcher<>), AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFromAssemblies(typeof(ICommandHandler<>), AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFromAssemblies(typeof(IQueryHandler<,>), AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICommandBus, CommandBus>();
builder.Services.AddScoped<IQueryBus, QueryBus>();

builder.Services.AddKeycloakAuthentication(builder.Configuration, configureOptions =>
{
	// turning off issuer validation and https
	configureOptions.RequireHttpsMetadata = false;
	configureOptions.TokenValidationParameters.ValidateIssuer = false;
});
builder.Services.AddAuthorization();

builder.Services.AddSwagger();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseCors(corsPolicyName);

app.UseHttpLogging();
app.UseHttpsRedirection();

app.MapControllers();

app.UseExampleModule();
app.UseBudgetModule();
app.UseUserModule();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();