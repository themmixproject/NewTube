using NewTube.Shared.DataTransfer;
using System.Net.Http.Json;

namespace NewTube.Client.Clients
{
    public class AuthClient
    {
        private readonly string _baseUrl = "auth/";
        private readonly HttpClient _httpClient;
        
        public AuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void RequestLogin(LoginRequest loginRequest)
        {
            _httpClient.PostAsJsonAsync($"{_baseUrl}login", loginRequest);
        }
    }
}
