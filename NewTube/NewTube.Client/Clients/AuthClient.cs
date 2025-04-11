using System.Net.Http.Json;
using System.Text.Json;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;

namespace NewTube.Client.Clients
{
    public class AuthClient : IAuthService
    {
        private readonly string _endPoint = "auth";
        private readonly HttpClient _httpClient;
        
        public AuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> RequestLoginAsync(LoginRequest loginRequest)
        {
            var result = await _httpClient.PostAsJsonAsync($"{_endPoint}/login", loginRequest);
            var contentString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponse>(contentString)!;
        }

        public async Task<SignUpResponse> RequestSignUpAsync(SignUpRequest signUpRequest)
        {
            var result = await _httpClient.PostAsJsonAsync($"{_endPoint}/signup", signUpRequest);
            var contentString = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<SignUpResponse>(contentString)!;
        }
    }
}
