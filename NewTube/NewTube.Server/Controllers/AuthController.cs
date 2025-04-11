using Microsoft.AspNetCore.Mvc;

namespace NewTube.Server.Controllers
{
    [Route("[controller]")]
    public class AuthController
    {
        public AuthController() { }

        [HttpPost("login")]
        public string LoginUser()
        {
            return "User has been logged in!";
        }
    }
}
