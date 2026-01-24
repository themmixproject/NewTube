using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NewTube.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthStatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAuthStatus()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new
                {
                    IsAuthenticated = true,
                    UserName = User.Identity.Name,
                    Claims = User.Claims.Select(c => new { c.Type, c.Value })
                });
            }

            return Ok(new { IsAuthenticated = false });
        }
    }
}