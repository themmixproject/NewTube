using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using NewTube.Cookies.Interfaces;
using NewTube.Cookies.Models;
using System.Text.Json;

namespace NewTube.Cookies.Server.Services
{
    internal class HttpContextCookieService : ICookieService
    {
        private readonly HttpContext _httpContext;
        private readonly Dictionary<string, Cookie> _requestCookies;
        private IHeaderDictionary ResponseHeaders { get; set; }

        public HttpContextCookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext!;
            _requestCookies = _httpContext.Request.Cookies.Select(c => new Cookie(c.Key, c.Value)).ToDictionary(c => c.Key);
        }

        public Task<IEnumerable<Cookie>> GetAllAsync()
        {
            return Task.FromResult(_requestCookies.Select(x => x.Value).ToList().AsEnumerable());
        }

        public Task<Cookie?> GetAsync(string key)
        {
            if (_requestCookies.TryGetValue(key, out var cookie)) return Task.FromResult<Cookie?>(cookie);

            return Task.FromResult<Cookie?>(null);
        }

        public Task<Cookie<T>?> GetAsync<T>(string key, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            if (_requestCookies.TryGetValue(key, out var cookie))
            {
                return Task.FromResult<Cookie<T>?>(cookie.Cast<T>(jsonSerializerOptions));
            }
            return Task.FromResult<Cookie<T>?>(null);
        }

        public Task SetAsync(
            string key,
            string value,
            DateTimeOffset? expiration = null,
            bool httpOnly = false,
            bool secure = false,
            Enums.SameSiteMode? sameSiteMode = null,
            CancellationToken cancellationToken = default
        ) {
            return SetAsync(
                new Cookie(key, value, expiration, httpOnly, secure, sameSiteMode),
                cancellationToken
            );
        }
        public Task SetAsync(Cookie cookie, CancellationToken cancellationToken = default)
        {
            if (cookie.Secure && !cookie.HttpOnly) throw new InvalidOperationException("Unable to set a cookie: Secure cookies must also be HttpOnly.");

            RemovePending(cookie.Key);

            _httpContext.Response.Cookies.Append(cookie.Key, cookie.Value, new CookieOptions
            {
                Expires = cookie.Expiration,
                Path = "/",
                HttpOnly = cookie.HttpOnly,
                Secure = cookie.Secure,
                SameSite = cookie.SameSiteMode.ToHttp()
            });

            return Task.CompletedTask;
        }

        private bool RemovePending(string key)
        {
            var cookieValues = ResponseHeaders
                .SetCookie
                .ToList();

            for (int i = 0; i < cookieValues.Count; i++)
            {
                var value = cookieValues[i];
                if (string.IsNullOrWhiteSpace(value)) continue;
                if (!value.StartsWith($"{key}=")) continue;

                cookieValues.RemoveAt(i);
                ResponseHeaders.SetCookie = new([.. cookieValues]);

                return true;
            }

            return false;
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            if (_requestCookies.Remove(key))
            {
                _httpContext.Response.Cookies.Delete(key);
            }

            return Task.CompletedTask;
        }

    }
}
