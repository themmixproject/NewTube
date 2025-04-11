using Microsoft.AspNetCore.Identity;
using NewTube.Server.Data;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;

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

        public async Task<LoginResponse> RequestLoginAsync(LoginRequest loginRequest)
        {
            SignInResult result = await SignInManager.PasswordSignInAsync(
                loginRequest.Username,
                loginRequest.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            LoginResponse response = new LoginResponse();
            if (result.Succeeded) {
                response.IsSuccessful = true;
            }

            return response;
        }

        public async Task<SignUpResponse> RequestSignUpAsync(SignUpRequest request)
        {
            ApplicationUser user = new ApplicationUser();
            await UserStore.SetUserNameAsync(user, request.Email, CancellationToken.None);

            IUserEmailStore<ApplicationUser> emailStore = GetEmailStore();
            await emailStore.SetEmailAsync(user, request.Email, CancellationToken.None);

            IdentityResult result = await UserManager.CreateAsync(user, request.Password);

            SignUpResponse response = new SignUpResponse();
            if (result.Succeeded)
            {
                response.IsSuccessful = true;
            }

            return response;
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
