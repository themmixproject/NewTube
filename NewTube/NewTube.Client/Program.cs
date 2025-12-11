using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NewTube.Client;
using NewTube.Client.Services;
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
            builder.Services.AddSingleton<AuthenticationStateProvider, ClientAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthenticationService, AuthService>();

            await builder.Build().RunAsync();
        }
    }
}
