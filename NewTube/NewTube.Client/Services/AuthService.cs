
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using MMIX.Blazor.Cookies;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;

namespace NewTube.Client.Services;

public class AuthService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ICookieService _cookieService;

    public AuthService(
        AuthenticationStateProvider authenticationStateProvider,
        HttpClient httpClient,
        ICookieService cookieService
    ) {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _cookieService = cookieService;
    }

    public async Task<SignUpResponse> RegisterUserAsync(SignUpRequest signUpRequest)
    {
        return await _httpClient.PostAsJsonAsync<SignUpResponse>("api/accounts", signUpRequest);
    }

    public async Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest)
    {
        var loginJSON = JsonSerializer.Serialize(loginRequest); 
        var response = await _httpClient.PostAsync("api/login", new StringContent(loginJSON, Encoding.UTF8, "applicatoin/json"));
        var loginResult = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync());

        if (!loginResult.isSuccessful)
        {
            return loginResult;
        }

        await _cookieService.SetAsync("authToken", loginResult.token);
        ((ClientAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginRequest.UserName);

        return loginResult;
    }

    public async Task Logout()
    {
        await _cookieService.RemoveAsync("authToken");
        ((ClientAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
    }
}
