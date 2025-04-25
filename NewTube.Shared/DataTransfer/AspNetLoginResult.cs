using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTube.Shared.DataTransfer
{
    public class AspNetLoginResult
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
