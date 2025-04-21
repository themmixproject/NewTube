using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using NewTube.Server.Data;
using System.Security.Claims;

namespace NewTube.Server.Controllers
{
    [Route("[controller]")]
    public class ManageController
    {
        private readonly SignInManager<ApplicationUser> SingInManager;
        private readonly UserManager<ApplicationUser> UserManager;
        public ManageController (SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            SingInManager = signInManager;
            UserManager = userManager;
        }

        private async Task<InfoResponse> CreateInfoResponseAsync(ApplicationUser user)
        {
            return new InfoResponse
            {
                Email = await UserManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
                IsEmailConfirmed = await UserManager.IsEmailConfirmedAsync(user)
            };
        }

        [HttpGet("info")]
        public async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>> GetUserInfoAsync(ClaimsPrincipal claimsPrincipal)
        {
            if (await UserManager.GetUserAsync(claimsPrincipal) is not { } user)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(await CreateInfoResponseAsync(user));
        }
    }
}
