using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewTube.Cookies.Client.Services;
using NewTube.Cookies.Interfaces;
using NewTube.Cookies.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTube.Cookies.Server.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCookieService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<JsInteropCookiesService>();
            services.AddScoped<HttpContextCookieService>();
            services.AddScoped<ICookieService>(i =>
            {
                IHttpContextAccessor? httpContextAccessor = i.GetRequiredService<IHttpContextAccessor>();
                HttpContext? httpContext = httpContextAccessor.HttpContext;
                bool isPrerendering = (httpContext != null && !httpContext.Response.HasStarted);

                if (isPrerendering)
                {
                    return i.GetRequiredService<HttpContextCookieService>();
                }
                else {
                    return i.GetRequiredService<JsInteropCookiesService>(); 
                }
            });

            return services;
        }
    }
}
