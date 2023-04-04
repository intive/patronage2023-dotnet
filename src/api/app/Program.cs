using Intive.Patronage2023.Api.Configuration;
using Intive.Patronage2023.Modules.Example.Api;
using Intive.Patronage2023.Modules.Example.Api.User;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
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

builder.Services.AddHttpLogging(logging =>
{
	logging.LoggingFields = HttpLoggingFields.All;
	logging.RequestBodyLogLimit = 4096;
	logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddSharedModule();
builder.Services.AddExampleModule(builder.Configuration);
builder.Services.AddUserModule(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
	cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddControllers(options =>
	options.Filters.Add(new AuthorizeFilter(
		new AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser()
			.Build())));

builder.Services.AddFromAssemblies(typeof(IDomainEventHandler<>));
builder.Services.AddFromAssemblies(typeof(IEventDispatcher<>));
builder.Services.AddFromAssemblies(typeof(ICommandHandler<>));
builder.Services.AddFromAssemblies(typeof(IQueryHandler<,>));

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

var app = builder.Build();

app.UseCors(corsPolicyName);

app.UseHttpLogging();
app.UseHttpsRedirection();

app.MapControllers();

app.UseExampleModule();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();