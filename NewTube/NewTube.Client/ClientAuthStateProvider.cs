using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace NewTube.Client
{
    internal class ClientAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient HttpClient;

        public ClientAuthenticationStateProvider(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var response = await HttpClient.GetAsync("/_auth/me");
            if (response == null) { return new AuthenticationState(new ClaimsPrincipal()); }

            var responseContent = await response.Content.ReadAsStringAsync();
            var principal = JsonSerializer.Deserialize<ClaimsPrincipal>(responseContent)!;

            return new AuthenticationState(principal);
        }
    }
}
