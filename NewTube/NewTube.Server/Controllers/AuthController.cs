using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;
using NewTube.Server.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using NewTube.Server.Data;
using System.Security.Claims;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        private class RoleClaimResult
        {
            public string Issuer { get; set; }
            public string OriginalIssuer { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
            public string ValueType { get; set; }
        }

        [HttpGet("roles")]
        public async Task<Results<JsonHttpResult<IEnumerable<RoleClaimResult>>, UnauthorizedHttpResult>> GetUserRoles(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null && claimsPrincipal.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)claimsPrincipal.Identity;
                var roles = identity.FindAll(identity.RoleClaimType).Select( claim => new RoleClaimResult
                {
                    Issuer = claim.Issuer,
                    OriginalIssuer = claim.OriginalIssuer,
                    Type = claim.Type,
                    Value = claim.Value,
                    ValueType = claim.ValueType
                });

                return TypedResults.Json(roles);
            }

            return TypedResults.Unauthorized();
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
