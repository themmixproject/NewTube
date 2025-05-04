using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NewTube.Shared.Interfaces;
using System.Net.Http;

namespace NewTube.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();

            builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
            builder.Services.AddScoped(sp => (IAuthService)sp.GetRequiredService<AuthenticationStateProvider>());

            builder.Services.AddTransient<CookieHandler>();
            builder.Services.AddScoped<HttpClient>(sp =>
            {
                var httpClient = HttpClientFactory.Create(sp.GetRequiredService<CookieHandler>());
                httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                return httpClient;
            });

            await builder.Build().RunAsync();
        }
    }
}
