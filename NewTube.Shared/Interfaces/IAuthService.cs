using NewTube.Shared.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTube.Shared.Interfaces
{
    internal class IAuthService
    {
        public void RequestLogin(LoginRequest loginRequest);
    }
}
