using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VagtplanApp.Client;
using VagtplanApp.Client.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Tilføjer HttpClient for at kunne foretage HTTP-anmodninger
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// Tilføjer PersonService som en scoped service, der bruger HttpClient - hvorfor egentlig 
builder.Services.AddHttpClient<IPersonService, PersonService>(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

// Tilføjer Blazored LocalStorage for at kunne lagre data lokalt i browseren
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
