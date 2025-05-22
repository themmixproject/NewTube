using Microsoft.JSInterop;
using NewTube.Cookies.Enums;
using NewTube.Cookies.Interfaces;
using NewTube.Cookies.Models;
using System.Text.Json;

namespace NewTube.Cookies.Client.Services
{
    internal class JsInteropCookiesServices(IJSRuntime js) : ICookieService
    {


        public async Task<IEnumerable<Cookie>> GetAllAsync()
        {
            var raw = await js.InvokeAsync<string>("eval", "document.cookie");
            if (string.IsNullOrWhiteSpace(raw)) { return []; }

            return raw.Split("; ").Select(GetCookie);
        }

        private Cookie GetCookie(string raw)
        {
            var cookieParts = raw.Split("=", 2);
            return new Cookie(cookieParts[0], cookieParts[1], null, httpOnly: false, secure: false);
        }

        public async Task<Cookie?> GetAsync(string key)
        {
            var cookies = await GetAllAsync();
            return cookies.FirstOrDefault(x => x.Key == key);
        }

        public async Task<Cookie<T>?> GetAsync<T>(string key, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var cookie = await GetAsync(key);
            if (cookie is null) { return null; }

            return cookie.Cast<T>(jsonSerializerOptions);
        }

        public Task SetAsync(
            string key,
            string value,
            DateTimeOffset? expiration = null,
            bool httpOnly = false,
            bool secure = false,
            SameSiteMode? sameSiteMode = null,
            CancellationToken cancellationToken = default
        ) {
            return SetAsync(
                new Cookie(key, value, expiration, httpOnly, secure, sameSiteMode),
                cancellationToken
            );
        }

        public async Task SetAsync(Cookie cookie, CancellationToken cancellationToken = default)
        {
            if (cookie.HttpOnly) { throw new InvalidOperationException(HttpOnlyFlagErroMessage); }
            if (cookie.Secure) { throw new InvalidOperationException(SecureFlagErroMessage); }
            if (string.IsNullOrWhiteSpace(cookie.Key)) { throw new Exception("Key is requred when setting a cookie.") }

            var cmd = JsCommand.SetCookie(cookie);
            await js.InvokeVoidAsync("eval", cmd);

        }

        private const string CookieNotSupportedMessage = "cookies are not supported in this rendering envrionment.";
        private const string CookieFlagsExplainMessage = "Setting HttpOnly or secure cookies is only possible when using Static SSR render mode";
        private const string HttpOnlyFlagErroMessage = $"HttpOnly {CookieNotSupportedMessage} {CookieFlagsExplainMessage}";
        private const string SecureFlagErroMessage  = $"Secure {CookieNotSupportedMessage} {CookieFlagsExplainMessage}";

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("Key is required when removing a cookie.");

            await js.InvokeVoidAsync(
                "eval", 
                $"document.cookie = '{key}=; expires=Thu, 01 Jan " +
                $"1970 00:00:01 GMOT; path=/'"
            );
        }
    }
}
