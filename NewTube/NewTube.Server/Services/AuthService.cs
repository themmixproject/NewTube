using Microsoft.AspNetCore.Identity;
using NewTube.Server.Data;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;
using System.Security.Claims;

namespace NewTube.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly IUserStore<ApplicationUser> UserStore;
        private readonly UserManager<ApplicationUser> UserManager;

        public AuthService(
            SignInManager<ApplicationUser> signInManager,
            IUserStore<ApplicationUser> userStore,
            UserManager<ApplicationUser> userManager
            )
        {
            SignInManager = signInManager;
            UserStore = userStore;
            UserManager = userManager;
        }

        public async Task<RequestResponse> RequestLoginAsync(string email, string password)
        {
            SignInResult result = await SignInManager.PasswordSignInAsync(
                email,
                password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            RequestResponse response = new RequestResponse();
            if (result.Succeeded)
            {
                response.IsSuccessful = true;
            }

            return response;
        }

        public async Task<RequestResponse> RequestLogoutAsync()
        {
            await SignInManager.SignOutAsync();
           
            return new RequestResponse{ IsSuccessful = true };
        }

        public async Task<RequestResponse> RequestSignUpAsync(string email, string password)
        {
            ApplicationUser user = new ApplicationUser();
            await UserStore.SetUserNameAsync(user, email, CancellationToken.None);

            IUserEmailStore<ApplicationUser> emailStore = GetEmailStore();
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);

            IdentityResult result = await UserManager.CreateAsync(user, password);

            RequestResponse response = new RequestResponse();
            if (result.Succeeded)
            {
                response.IsSuccessful = true;
            }

            return response;
        }

        public async Task<bool> CheckIfAuthenticatedAsync()
        {
            var result=await UserManager.FindByIdAsync("");
            return false;
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!UserManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)UserStore;
        }
    }
}
