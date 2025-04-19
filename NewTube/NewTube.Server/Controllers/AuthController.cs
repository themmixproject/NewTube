using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;
using NewTube.Server.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using NewTube.Server.Data;

namespace NewTube.Server.Controllers
{
    [Route("[controller]")]
    public class AuthController
    {
        private readonly SignInManager<ApplicationUser> SignInManager;

        public AuthController(SignInManager<ApplicationUser> signInManager)
        {
            SignInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<Results
            <Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> LoginUser
            (
                [FromBody] LoginRequest login,
                [FromQuery] bool? useCookies,
                [FromQuery] bool? useSessionCookies
            )
        {
            var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
            var isPersistent = (useCookies == true) && (useSessionCookies != true);
            SignInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

            var result = await SignInManager.PasswordSignInAsync(
                login.Username,
                login.Password,
                isPersistent,
                lockoutOnFailure: true
            );

            if (!result.Succeeded)
            {
                return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
            }

            return TypedResults.Empty;
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
