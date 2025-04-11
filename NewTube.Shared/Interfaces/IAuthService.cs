using NewTube.Shared.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTube.Shared.Interfaces
{
    public interface IAuthService
    {
        public void RequestLogin(LoginRequest loginRequest);
    }
}
