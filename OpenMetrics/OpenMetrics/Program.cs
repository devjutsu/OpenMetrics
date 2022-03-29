using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MetaMask.Blazor;
using Blazored.Toast;
using OpenMetrics;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenMetrics.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

long netId = 1;
long.TryParse(builder.Configuration["NetworkId"], out netId);

var config = new ClientConfig()
{
    ApiUrl = builder.Configuration["ApiUrl"] ?? "",
    NetworkId = netId,
    ContractAddress = builder.Configuration["ContractAddress"],
    RpcUrl = builder.Configuration["RpcUrl"],
};

builder.Services.AddSingleton<ClientConfig>(config);
builder.Services.AddMetaMaskBlazor();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<AppState>();

await builder.Build().RunAsync();
