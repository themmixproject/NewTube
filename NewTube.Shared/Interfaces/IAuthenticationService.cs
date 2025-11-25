using NewTube.Shared.DataTransfer;

namespace NewTube.Shared.Interfaces;

public interface IAuthenticationService
{
    public Task<LoginResponse> LoginUser();
    public void RegisterUser();
}
