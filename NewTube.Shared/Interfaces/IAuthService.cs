using NewTube.Shared.DataTransfer;

namespace NewTube.Shared.Interfaces
{
    public interface IAuthService
    {
        public Task<LoginResponse> RequestLoginAsync(LoginRequest loginRequest);
        public Task RequestLogoutAsync();
        public Task<SignUpResponse> RequestSignUpAsync(SignUpRequest signUpRequest);
    }
}
