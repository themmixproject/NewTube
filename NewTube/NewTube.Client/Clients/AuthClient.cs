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

        public void RequestLogin()
        {
            _httpClient.PostAsync($"{_baseUrl}login", null);
        }
    }
}
