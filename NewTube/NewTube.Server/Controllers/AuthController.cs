using Microsoft.AspNetCore.Mvc;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;

namespace NewTube.Server.Controllers
{
    [Route("[controller]")]
    public class AuthController
    {
        private IAuthService AuthService;

        public AuthController(IAuthService authService)
        {
            AuthService = authService;
        }

        [HttpPost("login")]
        public async Task<LoginResponse> LoginUser([FromBody] LoginRequest loginRequest)
        {
            return await AuthService.RequestLoginAsync(loginRequest);
        }
    }
}
