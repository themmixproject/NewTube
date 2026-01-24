using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewTube.Server.Data;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;

namespace NewTube.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginController(
            IAuthenticationService authenticationService,
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            LoginResponse loginResponse = await _authenticationService.LoginUserAsync(loginRequest);

            if (!loginResponse.isSuccessful)
            {
                return BadRequest(loginResponse);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, loginRequest.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: credentials
            );

            loginResponse.token = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(loginResponse);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { success = true });
        }
    }
}
