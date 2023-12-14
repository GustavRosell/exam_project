using Microsoft.AspNetCore.ResponseCompression;
using MongoDB.Driver;
using VagtplanApp.Client.Services;
using VagtplanApp.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Registrerer repositories som singleton-tjenester for at sikre en enkelt, delt instans igennem applikationens livscyklus.
builder.Services.AddSingleton<IPersonRepository, PersonRepository>();
builder.Services.AddSingleton<IShiftRepository, ShiftRepository>();

var app = builder.Build();

// Konfigurerer HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
