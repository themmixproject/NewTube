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
