using NewTube.Shared.Interfaces;
using NewTube.Shared.DataTransfer;
using Microsoft.AspNetCore.Identity;
using NewTube.Server.Data;
using System.Threading.Tasks;

namespace NewTube.Server.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthenticationService(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<LoginResponse> LoginUser(LoginRequest loginRequest)
    {
        var result = await _signInManager.PasswordSignInAsync(loginRequest.UserName, loginRequest.Password, true, false);
    
        var response = new LoginResponse{isSuccessful = false};
        if (result.Succeeded) {response.isSuccessful = true;}

        return response;
    }
    public void RegisterUser() {}
}
