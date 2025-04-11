using Microsoft.AspNetCore.Mvc;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;

namespace NewTube.Server.Controllers
{
    [Route("[controller]")]
    public class AuthController
    {
        private IAuthService AuthService

        public AuthController(IAuthService authService)
        {
            AuthService = authService;
        }

        [HttpPost("login")]
        public string LoginUser([FromBody] LoginRequest loginRequest)
        {
            AuthService.RequestLogin(loginRequest);
            return "User has been logged in!";
        }
    }
}
