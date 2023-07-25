using Blazored.LocalStorage;
using BlazorVeiculos;
using BlazorVeiculos.Servicos.Autenticacao;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ApiVeiculos", options =>
{
    options.BaseAddress = new Uri("http://localhost:5283/");
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, ApiAutenticacaoStateProvider>();    

await builder.Build().RunAsync();
