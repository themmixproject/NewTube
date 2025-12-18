using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewTube.Server.Data;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;

namespace NewTube.Server.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticationService _authenticationService;

        public AccountsController(
            IAuthenticationService authenticationService,
            UserManager<ApplicationUser> userManager
        ) {
            _authenticationService = authenticationService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult HttpGet()
        {
            return Ok("hello world!");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync([FromBody] SignUpRequest signUpRequest)
        {
            SignUpResponse signUpResponse = await _authenticationService.RegisterUserAsync(signUpRequest);

            return Created("", signUpResponse);
        }
    }
}
