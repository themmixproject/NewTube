using NewTube.Shared.DataTransfer;

namespace NewTube.Shared.Interfaces
{
    public interface IAuthService
    {
        public Task RequestLoginAsync(LoginRequest loginRequest);
        public Task RequestLogoutAsync();
        public Task RequestSignUpAsync(SignUpRequest signUpRequest);
    }
}
