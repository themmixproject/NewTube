using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NewTube.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Checks the health status of the API and returns a response indicating whether it is online.
        /// </summary>
        /// <param name="cancellationToken">Used to signal cancellation of the operation if needed.</param>
        /// <returns>
        /// Returns a 200 OK response with a health message if in development or local environment; otherwise, returns a 200
        /// OK without a message.
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetHealth(CancellationToken cancellationToken)
        {
            return Ok("API is online");
        }

    }
}
