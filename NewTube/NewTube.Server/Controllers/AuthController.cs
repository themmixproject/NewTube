using Microsoft.AspNetCore.Mvc;

namespace NewTube.Server.Controllers
{
    [Route("[controller]")]
    public class AuthController
    {
        public AuthController() { }

        [HttpGet("test")]
        public string Test()
        {
            return "Hello World!";
        }
    }
}
