using QuizWars.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using QuizWars.Sdk;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("QuizWars.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("QuizWars.ServerAPI"));
builder.Services.AddScoped(sp => new QuizWarsClient(sp.GetRequiredService<HttpClient>()));

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
