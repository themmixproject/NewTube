using NewTube.Shared.DataTransfer;

namespace NewTube.Shared.Interfaces;

public interface IAuthenticationService
{
    public Task<LoginResponse> LoginUserAsync();
    public void RegisterUserAsync();
}
