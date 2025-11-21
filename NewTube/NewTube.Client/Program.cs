using BitzArt.Blazor.Auth.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NewTube.Client;

namespace NewTube.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.AddBlazorAuth();

            await builder.Build().RunAsync();
        }
    }
}
