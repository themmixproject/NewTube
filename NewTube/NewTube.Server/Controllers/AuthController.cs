using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace NewTube.Server.Controllers
{
    [Route("[controller]")]
    public class AuthController
    {
        public AuthController() { }

        [HttpPost("login")]
        public string LoginUser([FromBody] LoginRequest loginRequest)
        {
            Console.WriteLine(loginRequest);
            return "User has been logged in!";
        }
    }
}
