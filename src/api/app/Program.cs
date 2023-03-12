using Intive.Patronage2023.Modules.Example.Api;

var builder = WebApplication.CreateBuilder(args);

// Set of logging providers added by WebApplication.CreateBuilder
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configure JSON logging to the console.
builder.Logging.AddJsonConsole();
builder.Services.AddRazorPages();

builder.Services.AddExampleModule();

var app = builder.Build();

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

app.UseExampleModule();

app.Run();