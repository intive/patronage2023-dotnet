using Intive.Patronage2023.Modules.Example.Api;

// Zastępuje domyślny zestaw dostawców rejestrowania dodanych przez metodę WebApplication.CreateBuilder
var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddExampleModule();
builder.Services.AddRazorPages();

var app = builder.Build();

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