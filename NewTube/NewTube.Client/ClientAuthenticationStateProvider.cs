using System;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using MMIX.Blazor.Cookies;

namespace NewTube.Client;

public class ClientAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public ClientAuthenticationStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/authstatus");
            if (response.IsSuccessStatusCode)
            {
                var authStatusJson = await response.Content.ReadAsStringAsync();
                var authStatus = JsonSerializer.Deserialize<AuthStatusResponse>(authStatusJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (authStatus?.IsAuthenticated == true)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, authStatus.UserName ?? "")
                    };

                    // Add additional claims if available
                    if (authStatus.Claims != null)
                    {
                        claims.AddRange(authStatus.Claims.Select(c => new Claim(c.Type, c.Value)));
                    }

                    var identity = new ClaimsIdentity(claims, "cookie");
                    return new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
        }
        catch
        {
            // If there's any error, return anonymous user
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }


    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyAuthenticationStateChanged()
    {
        var authStateTask = GetAuthenticationStateAsync();
        base.NotifyAuthenticationStateChanged(authStateTask);
    }
}

// Response model for the auth status endpoint
public class AuthStatusResponse
{
    public bool IsAuthenticated { get; set; }
    public string? UserName { get; set; }
    public List<ClaimInfo>? Claims { get; set; }
}

public class ClaimInfo
{
    public string Type { get; set; } = "";
    public string Value { get; set; } = "";
}
