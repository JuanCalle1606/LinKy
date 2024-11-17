using Linky.Client.Extensions;
using Linky.Client.Pages;
using Linky.Client.Services;
using Linky.Components;
using Linky.Models;
using Linky.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();

builder.Services.AddSharedServices();

builder.Services.AddDbContext<LinkyDbContext>(options =>
	options.UseSqlite(config.GetConnectionString("UrlDb")));

builder.Services.AddControllers();

builder.Services.AddScoped<ILinkManager, ServerLinkManager>();
builder.Services.AddHostedService<DbInit>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(Linky.Client._Imports).Assembly);

app.MapControllers();

app.Run();