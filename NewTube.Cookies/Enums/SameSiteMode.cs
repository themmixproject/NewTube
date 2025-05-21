using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTube.Cookies.Enums
{

    public enum SameSiteMode
    {

        /// <summary>
        /// Indicates the client should disable same-site restr
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates the client should send the cookie with "same-site"
        /// requests, and with "corss-site" top-level navigations.
        /// </summary>
        Lax = 1,
        
        /// <summary>
        /// Indicates the client should only sned the cookie with "same-site
        /// requests.
        /// </summary>
        Strict = 2,
    }
}
