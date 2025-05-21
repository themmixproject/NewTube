using NewTube.Cookies.Enums;
using NewTube.Cookies.Models;
using System.Text.Json;

namespace NewTube.Cookies.Interfaces
{
    /// <summary>
    /// Allows interacting with browser cookies.
    /// </summary>
    internal interface ICookieService
    {
        /// <summary>
        /// Retrieves all cookies.
        /// <para>
        /// <b>Note: </b> When retrieving a cookie, certain properties of the
        /// resulting cookie object may be unavailable. This is because
        /// browsers do not expose these attributes of cookies to neither
        /// client-side or server-side code. Only the cookie's key and value
        /// are accessible, with the browser keeping other attributes (such as
        /// 'HttpOnly', 'Secure', and 'SameSite') hidden for security and
        /// privacy reasons.
        /// </para>
        /// </summary>
        public Task<IEnumerable<Cookie>> GetAllAsync();

        /// <summary>
        /// Retrieves a cookie by it's key.
        /// <para>
        /// <b>Note: </b> When retrieving a cookie, certain properties of the
        /// resulting cookie object may be unavailable. This is because
        /// browsers do not expose these attributes of cookies to neither
        /// client-side or server-side code. Only the cookie's key and value
        /// are accessible, with the browser keeping other attributes (such as
        /// 'HttpOnly', 'Secure', and 'SameSite') hidden for security and
        /// privacy reasons.
        /// </para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<Cookie?> GetAsync(string key);
        /// <inheritdoc cref="GetAsync(string){T}(string, JsonSerializerOptions)"/>
        public Task<Cookie<T>?> GetAsync<T>(string key, JsonSerializerOptions? jsonSerializerOptions = null);
        
        /// <summary>
        /// Removes a cookie by marking it as expired.
        /// </summary>
        /// <param name="key">The key of the cookie to remove.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task RemoveAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Name of the cookie.</param>
        /// <param name="value">Value of the cookie.</param>
        /// <param name="expiration">Value of the cookie.</param>
        /// <param name="httpOnly">
        /// Whether the cookie is inaccessible by the client-side script.
        /// </param>
        /// <param name="secure">
        /// Whether to transmit the cookie using Secure Sockets Layer
        /// (SSL)--that is, over HTTPS only.
        /// </param>
        /// <param name="sameSiteMode">
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie#samesitesamesite-value">SameSiteMode</see>
        /// controls whether or not a cookie is sent with cross-site requests,
        /// providing some protection against cross-site request forgery attacks
        /// (<see href="https://developer.mozilla.org/en-US/docs/Glossary/CSRF">CSRF</see>).
        /// <br />
        /// <b>Note:</b> Null value will result in the browser using it's default
        /// behavior.
        /// </param>
        /// <param name="cancellationToken"> Cancellation token. </param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SetAsync(
            string key,
            string value,
            DateTimeOffset? expiration = null,
            bool httponly = false,
            bool secure = false,
            SameSiteMode? sameSiteMode = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Adds or updates a browser cookie.
        /// <br />
        /// <br />
        /// <b>Note: </b>When in <see href="https://learn.microsoft.com/en-US/aspnet/core/blazor/components/render-modes">Static SSR render mode</see>,
        /// the new value will only be sent to the client machine after the
        /// page has completed rendering, and this will not appear in the
        /// cookies collection until the next request.
        /// </summary>
        /// <param name="cookie">The cookie to set.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SetAsync(Cookie cookie, CancellationToken cancellationToken = default);
    }
}
