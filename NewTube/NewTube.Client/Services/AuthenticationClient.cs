
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using MMIX.Blazor.Cookies;
using NewTube.Client.Components.Pages;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;

namespace NewTube.Client.Services;

public class AuthenticationClient : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ICookieService _cookieService;

    public AuthenticationClient(
        AuthenticationStateProvider authenticationStateProvider,
        HttpClient httpClient,
        ICookieService cookieService
    ) {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _cookieService = cookieService;
    }

    public async Task<SignUpResponse> RegisterUserAsync(
        SignUpRequest signUpRequest,
        CancellationToken cancellationToken = default
    ) {
        var signUpJSON = JsonSerializer.Serialize(signUpRequest);
        var response = await _httpClient.PostAsync(
            "api/accounts",
            new StringContent(signUpJSON, Encoding.UTF8, "application/json"),
            cancellationToken
        );
        string responseString = await response.Content.ReadAsStringAsync();
        var signUpResult = JsonSerializer.Deserialize<SignUpResponse>(responseString);

        return signUpResult;
    }

    public async Task<LoginResponse> LoginUserAsync(
        LoginRequest loginRequest,
        CancellationToken cancellationToken = default
    ) {
        var loginJSON = JsonSerializer.Serialize(loginRequest); 
        var response = await _httpClient.PostAsync(
            "api/login",
            new StringContent(loginJSON, Encoding.UTF8, "application/json"),
            cancellationToken
        );
        var responseString = await response.Content.ReadAsStringAsync();
        var loginResult = JsonSerializer.Deserialize<LoginResponse>(responseString);

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
