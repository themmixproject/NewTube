using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;

namespace NewTube.Client;

public class CookieAuthenticationStateProvider : AuthenticationStateProvider, IAuthService
{
    private readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private HttpClient httpClient { get; set; }
    bool authenticated = false;
    private readonly ClaimsPrincipal unauthenticated = new(new ClaimsIdentity());

    public CookieAuthenticationStateProvider(HttpClient _httpClient)
    {
        httpClient = _httpClient;
    }

    public async Task<RequestResponse> RequestSignUpAsync(string email, string password)
    {
        string[] defaultDetail = ["An unknown error prevented registration from succeeding."];

        var result = await httpClient.PostAsJsonAsync("auth/register", new { email, password });

        if (result.IsSuccessStatusCode){
            return new RequestResponse { IsSuccessful = true };
        }

        return new RequestResponse { IsSuccessful = false };
    }

    public async Task<RequestResponse> RequestLoginAsync(string email, string password)
    {
        var result = await httpClient.PostAsJsonAsync("auth/login?useCookies=true", new { email, password });

        if (result.IsSuccessStatusCode)
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            return new RequestResponse { IsSuccessful = true };
        }

        return new RequestResponse { IsSuccessful = false };
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        authenticated = false;

        var user = unauthenticated;

        try
        {
            var userResponse = await httpClient.GetAsync("auth/manage/info");
            userResponse.EnsureSuccessStatusCode();

            var userJson = await userResponse.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson);

            if (userInfo != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userInfo.Email),
                    new Claim(ClaimTypes.Email, userInfo.Email)
                };

                claims.AddRange(
                    userInfo.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
                        .Select(c => new Claim(c.Key, c.Value)));

                var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
                user = new ClaimsPrincipal(id);
                authenticated = true;
            }
        }
        catch (Exception ex) { }

        return new AuthenticationState(user);
    }

    public async Task<RequestResponse> RequestLogoutAsync()
    {
        const string Empty = "{}";
        var emptyContent = new StringContent(Empty, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("logout", emptyContent);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return new RequestResponse { IsSuccessful = true};
    }

    public async Task<bool> CheckIfAuthenticatedAsync()
    {
        await GetAuthenticationStateAsync();
        return authenticated;
    }
}
