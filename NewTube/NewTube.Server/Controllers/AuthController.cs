using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;
using NewTube.Server.Services;

namespace NewTube.Server.Controllers
{
    [Route("[controller]")]
    public class AuthController
    {
        public AuthController() { }

        [HttpPost("login")]
        public IResult LoginUser([FromBody] LoginRequest loginRequest)
        {
            return Results.Ok("ok");
        }

        //[HttpPost("login")]
        //public async Task<LoginResponse> LoginUser([FromBody] LoginRequest loginRequest)
        //{
        //    return await AuthService.RequestLoginAsync(loginRequest);
        //}

        //[HttpGet("logout")]
        //public async Task<LogoutResponse> LogoutUser()
        //{
        //    return await AuthService.RequestLogoutAsync();
        //}

        //[HttpPost("signup")]
        //public async Task<SignUpResponse> SignUpUser([FromBody] SignUpRequest signUpRequest)
        //{
        //    return await AuthService.RequestSignUpAsync(signUpRequest);
        //}
    }
}
