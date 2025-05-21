using NewTube.Cookies.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace NewTube.Cookies.Models
{
    /// <inheritdoc cref="="Cookie{T}"/>
    public class Cookie
    {
        private protected string _value;
        
        /// <summary>
        /// Name of the cookie.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value of the cookie.
        /// </summary>
        public string Value {
            get => _value;
            set { _value = value; OnValueChanged(value); }
        }

        /// <summary>
        /// Cookie expiration date.
        /// </summary>
        public DateTimeOffset? Expiration { get; set; }
        
        /// <summary>
        /// Whether the cookie is inaccessible by client-side script.
        /// </summary>
        public bool HttpOnly { get; set; }

        /// <summary>
        /// Whether to transmit the cookie using Secure Sockets Layer
        /// (SSL)--that is, over HTTPS only.
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        /// <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Headers/Set-Cookie#samesitesamesite-value">SameSiteMode</see>
        /// Controls whether or not the cookie is sent with cross-site
        /// requests, providing some protection against corss-site request
        /// forgery attacks
        /// <see href="https://developer.mozilla.org/en-US/docs/Glossary/CSRF">CSRF</see>).
        /// <br />
        /// <br>Note: </br> Null value will result i the browser using it's
        /// default behavior.
        /// </summary>
        public SameSiteMode? SameSiteMode { get; set; }

        /// <summary>
        /// Creates a new <see cref="Cookie"/>
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
        public Cookie (
            string key,
            string value,
            DateTimeOffset? expiration = null,
            bool httpOnly = false,
            bool secure = false,
            SameSiteMode? sameSiteMode = null
        )
        {
            Key = key;
            _value = value;
            Expiration = expiration;
            HttpOnly = httpOnly;
            Secure = secure;
            SameSiteMode = sameSiteMode;
        }

        private protected virtual void OnValueChanged(string value) { }

        public static Cookie<T> FromValue<T>(
            string key,
            T value,
            DateTimeOffset? expiration = null,
            bool httpOnly = false,
            bool secure = false,
            SameSiteMode? sameSiteMode = null
        )
        {
            return new Cookie<T>(key, value, expiration, httpOnly, secure, sameSiteMode);
        }

        public Cookie<T> Cast<T>(JsonSerializerOptions? jsonSerializerOptions = null)
        {
            T cookieValue = JsonSerializer.Deserialize<T>(Value, jsonSerializerOptions ?? new())!;
            return new Cookie<T>(
                Key,
                cookieValue,
                Expiration,
                HttpOnly,
                Secure,
                SameSiteMode,
                jsonSerializerOptions
            );
        }
    }
}
