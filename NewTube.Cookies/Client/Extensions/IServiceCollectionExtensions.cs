using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NewTube.Cookies.Client.Services;
using NewTube.Cookies.Interfaces;

namespace NewTube.Cookies.Client.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCookieService(this IServiceCollection services)
        {
            services.AddScoped<ICookieService, JsInteropCookiesService>();
            
            return services;
        }
    }
}
