using Microsoft.AspNetCore.Identity;
using NewTube.Server.Data;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;

namespace NewTube.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> SignInManager;

        public AuthService(SignInManager<ApplicationUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public async Task<LoginResponse> RequestLoginAsync(LoginRequest loginRequest)
        {
            var result = await SignInManager.PasswordSignInAsync(
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
    }
}
