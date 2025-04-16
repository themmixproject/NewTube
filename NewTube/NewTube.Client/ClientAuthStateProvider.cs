using System.Security.Claims
using Microsoft.AspNetCore.Components.Authorization;

namespace NewTube.Client
{
    public class ClientAuthStateProvider : AuthenticationStateProvider
    {
        public Task<AuthenticationState> GetAuthenticationStateAsync(string username)
        {
            var identity = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, username)    
            ], "Custom Authentication");

            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
