using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NewTube.Client.Clients;
using NewTube.Shared.Interfaces;

namespace NewTube.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<AuthenticationStateProvider, ClientAuthStateProvider>();
            builder.Services.AddSingleton<IAuthService, ClientAuthStateProvider>();
            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });
            builder.Services.AddScoped<AuthClient>();

            await builder.Build().RunAsync();
        }
    }
}
