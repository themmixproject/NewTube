using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NewTube.Client.Services;
using NewTube.Shared.Interfaces;
using MMIX.Blazor.Cookies.Client;

namespace NewTube.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddSingleton<HttpClient>(serviceProvider =>
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                return client;
            });

            builder.Services.AddCookieService(ServiceLifetime.Singleton);

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<AuthenticationStateProvider, ClientAuthenticationStateProvider>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationClient>();

            await builder.Build().RunAsync();
        }
    }
}
