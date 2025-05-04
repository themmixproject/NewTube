using NewTube.Shared.DataTransfer;

namespace NewTube.Shared.Interfaces
{
    public interface IAuthService
    {
        public Task<RequestResponse> RequestLoginAsync(string email, string password);
        public Task<RequestResponse> RequestLogoutAsync();
        public Task<RequestResponse> RequestSignUpAsync(string email, string password);
        public Task<bool> CheckIfAuthenticatedAsync();
    }
}
