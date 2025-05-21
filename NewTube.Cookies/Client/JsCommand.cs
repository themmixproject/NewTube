using NewTube.Cookies.Models;
using System.Text;

namespace NewTube.Cookies.Client
{
    internal class JsCommand
    {
        public static string SetCookie(Cookie cookie)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"document.cookie = \"");
            stringBuilder.Append($"{cookie.Key}={cookie.Value};");
            stringBuilder.Append($"expires={cookie.Expiration:R};");

            stringBuilder.Append($"path=/");
            if (cookie.SameSiteMode.HasValue)
            {
                stringBuilder.Append(";");
                stringBuilder.Append($"SameSite={cookie.SameSiteMode.Value.ToString()}");
            }
                
            stringBuilder.Append("\"");
            return stringBuilder.ToString();
        }
    }
}
