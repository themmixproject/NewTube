using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NewTube.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace NewTube.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddTransient<CookieHandler>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<CookieAuthenticationStateProvider>());
            builder.Services.AddSingleton<IAuthService, CookieAuthenticationStateProvider>();

            builder.Services.AddScoped(sp => new HttpClient{
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            var httpClient = HttpClientFactory.Create(new CookieHandler());
            httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            builder.Services.AddScoped<HttpClient>(sp => httpClient);

            await builder.Build().RunAsync();
        }
    }
}
