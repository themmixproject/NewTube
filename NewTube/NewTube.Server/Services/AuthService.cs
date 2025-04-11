using Microsoft.AspNetCore.Identity;
using NewTube.Server.Data;
using NewTube.Shared.DataTransfer;

namespace NewTube.Server.Services
{
    public class AuthService
    {
        private readonly SignInManager<ApplicationUser> SignInManager;

        public AuthService(SignInManager<ApplicationUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public void RequestLogin(LoginRequest loginRequest)
        {
            SignInManager.PasswordSignInAsync(
                loginRequest.Username,
                loginRequest.Password,
                isPersistent: false,
                lockoutOnFailure: false
            ).Wait();
        }
    }
}
