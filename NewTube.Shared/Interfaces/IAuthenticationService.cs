using NewTube.Shared.DataTransfer;

namespace NewTube.Shared.Interfaces;

public interface IAuthenticationService
{
    public Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default);
    public Task<SignUpResponse> RegisterUserAsync(SignUpRequest signUpRequest, CancellationToken cancellationToken = default);
}
