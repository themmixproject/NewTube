using NewTube.Shared.DataTransfer;

namespace NewTube.Shared.Interfaces
{
    public interface IAuthService
    {
        public Task<RequestResponse> RequestLoginAsync(LoginRequest loginRequest);
        public Task<RequestResponse> RequestLogoutAsync();
        public Task<RequestResponse> RequestSignUpAsync(SignUpRequest signUpRequest);
        public Task<bool> CheckIfAuthenticatedAsync();
    }
}
