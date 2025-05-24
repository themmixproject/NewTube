using Microsoft.AspNetCore.Http;

namespace NewTube.Cookies.Server.Extensions
{
    internal static class SameSiteModeExtensions
    {
        public static Microsoft.AspNetCore.Http.SameSiteMode ToHttp(this NewTube.Cookies.Enums.SameSiteMode? sameSiteMode)
        {
            if (sameSiteMode == Enums.SameSiteMode.None) { return SameSiteMode.None; }
            else if (sameSiteMode == Enums.SameSiteMode.Lax) { return SameSiteMode.Lax; }
            else if (sameSiteMode == Enums.SameSiteMode.Strict) { return SameSiteMode.Strict; }
            else if (sameSiteMode == null) { return SameSiteMode.Unspecified; }
            else { throw new ArgumentOutOfRangeException(nameof(sameSiteMode), sameSiteMode, null); }
        }
    }
}
