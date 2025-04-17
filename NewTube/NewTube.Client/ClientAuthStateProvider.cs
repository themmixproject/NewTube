using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using NewTube.Client.Clients;

namespace NewTube.Client
{
    public class ClientAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthClient AuthClient;
        private readonly HttpClient HttpClient;
        public ClientAuthStateProvider(AuthClient authClient, HttpClient httpClient)
        {
            this.AuthClient = authClient;
            this.HttpClient = httpClient;
        }

        {
            var identity = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, username)    
            ], "Custom Authentication");

            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
