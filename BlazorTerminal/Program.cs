using BlazorTerminal.Services;
using BlazorTerminal;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorTerminal.Pages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.RootComponents.RegisterForJavaScript<Counter>(identifier: "counter");
builder.RootComponents.RegisterForJavaScript<BlazorTerminal.Pages.Index>(identifier: "index");

builder.RootComponents.RegisterForJavaScript<Video>(identifier: "video");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://127.0.0.1:7296") });
builder.Services.AddScoped<ICommandLineService, CommandLineService>();

await builder.Build().RunAsync();
