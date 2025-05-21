using NewTube.Cookies.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewTube.Cookies.Models
{
    /// <inheritdoc cref="Cookie(string, string, DateTimeOffset?, bool, bool, SameSiteMode?" />
    public class Cookie<T> : Cookie
    {
        private protected new T? _value;

        /// <summary>
        /// The value of the cookie.
        /// </summary>
        public T? Value
        {
            get => _value;
            set
            {
                _value = value;
                base._value = JsonSerializer.Serialize(value, JsonSerializerOptions.Default);
            }
        }

        private protected override void OnValueChanged(string value)
        {
            _value = JsonSerializer.Deserialize<T>(base.Value, JsonSerializerOptions);
        }

        /// <summary>
        /// JSON serialization options to use when serializing/deserializing
        /// the cookie value.
        /// </summary>
        public JsonSerializerOptions JsonSerializerOptions { get; set; }

        /// <inheritdoc cref="Cookie(string, string, DateTimeOffset?, bool, bool, SameSiteMode?)"/>
        public Cookie(
            string key,
            T? value,
            DateTimeOffset? expiration = null,
            bool httpOnly = false,
            bool secure = false,
            SameSiteMode? sameSiteMode = null,
            JsonSerializerOptions? jsonSerializerOptions = null
        ) : base(
            key,
            JsonSerializer.Serialize(value, jsonSerializerOptions ?? new()),
            expiration,
            httpOnly,
            secure,
            sameSiteMode
        )
        {
            _value = value;
            JsonSerializerOptions = jsonSerializerOptions ?? new();
        }
    }
}
