namespace NewTube.Client.Clients
{
    public class AuthClient
    {
        private string baseUrl = "auth/";
        private HttpClient httpClient;
        
        public AuthClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
    }
}
