using Microsoft.AspNetCore.Identity;
using NewTube.Shared.Interfaces;
using NewTube.Shared.DataTransfer;
using NewTube.Server.Data;

namespace NewTube.Server.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;

    public AuthenticationService(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore
    ) {
        _signInManager = signInManager;
        _userManager = userManager;
        _userStore = userStore;
    }

    public async Task<LoginResponse> LoginUserAsync(
        LoginRequest loginRequest,
        CancellationToken cancellationToken = default
    ) {
        var result = await _signInManager.PasswordSignInAsync(
            loginRequest.UserName,
            loginRequest.Password,
            true,
            false
        );
    
        var response = new LoginResponse{isSuccessful = false};
        if (result.Succeeded) {response.isSuccessful = true;}

        return response;
    }
    public async Task<SignUpResponse> RegisterUserAsync(
        SignUpRequest signUpRequest,
        CancellationToken cancellationToken = default
    ) {
        ApplicationUser user = new ApplicationUser();
        await _userStore.SetUserNameAsync(user, signUpRequest.UserName, cancellationToken);

        var result = await _userManager.CreateAsync(user, signUpRequest.PassWord);
        
        var response = new SignUpResponse { isSuccessful = false };
        
        if (result.Succeeded)
        {
            response.isSuccessful = true;
        }

        return response;
    }
}
