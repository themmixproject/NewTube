using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using NewTube.Shared.DataTransfer;
using NewTube.Shared.Interfaces;

namespace NewTube.Client.Clients
{
    public class AuthClient
    {
        private readonly string _endPoint = "auth";
        private readonly HttpClient HttpClient;
        private readonly ClientAuthStateProvider AuthenticationStateProvider;
        private readonly ILogger Logger;

        public AuthClient(
            HttpClient httpClient,
            ClientAuthStateProvider authenticationStateProvider,
            ILogger<AuthClient> logger)
        {
            Logger = logger;
            HttpClient = httpClient;
            AuthenticationStateProvider = authenticationStateProvider;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="username">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The result serialized to a <see cref="FormResult"/></returns>
        public async Task RequestSignUpAsync(SignUpRequest signUpRequest)
        {
            string[] defaultDetail = ["An unknown error prevented registration from succeeding."];

            try
            {
                var result = await HttpClient.PostAsJsonAsync(
                    "auth/resgister",
                    signUpRequest
                );

                if (result.IsSuccessStatusCode)
                {
                    //return new FormResult { Succeeded = true };
                }

                // body should contain details about why it failed
                var details = await result.Content.ReadAsStringAsync();
                var problemDetails = JsonDocument.Parse(details);
                var errors = new List<string>();
                var errorList = problemDetails.RootElement.GetProperty("errors");

                foreach (var errorEntry in errorList.EnumerateObject())
                {
                    if (errorEntry.Value.ValueKind == JsonValueKind.String)
                    {
                        errors.Add(errorEntry.Value.GetString()!);
                    }
                    else if (errorEntry.Value.ValueKind == JsonValueKind.Array)
                    {
                        errors.AddRange(
                            errorEntry.Value.EnumerateArray().Select(
                                    e => e.GetString() ?? string.Empty
                                ).Where(
                                    e => !string.IsNullOrEmpty(e))
                                );
                    }
                }

                //return new FormResult
                //{
                //    Succeeded = false,
                //    ErrorList = problemDetails == null ? defaultDetail : [.. errors]
                //};
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "App error");
            }

            //return new FormResult
            //{
            //    Succeeded = false,
            //    ErrorList = defaultDetail
            //};
        }

        /// <summary>
        /// User login.
        /// </summary>
        /// <param name="email">Hte user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The result of the login request serialized to a <see cref="FormResult"/></returns>
        public async Task RequestLoginAsync(LoginRequest loginRequest)
        {
            try
            {
                // login with cookies
                var result = await HttpClient.PostAsJsonAsync(
                    "auth/login?useCookies=true",
                    loginRequest
                );

                if (result.IsSuccessStatusCode)
                {
                    // need to refresh auth state
                    AuthenticationStateProvider.NotifyAuthenticationStateChanged(
                        AuthenticationStateProvider.GetAuthenticationStateAsync()
                    );

                    //return new FormResult { Succeeded = true };
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "App error");
            }

            // unknown error
            //return new FormResult
            //{
            //    Succeeded = false,
            //    ErrorList = ["Invalid email and/or password."]
            //};
        }

        public async Task RequestLogoutAsync()
        {
            var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
            await HttpClient.PostAsync("auth/logout", emptyContent);

            AuthenticationStateProvider.NotifyAuthenticationStateChanged(
                AuthenticationStateProvider.GetAuthenticationStateAsync()
            );
        }

        //public async Task<LoginResponse> RequestLoginAsync(LoginRequest loginRequest)
        //{
        //    var result = await _httpClient.PostAsJsonAsync($"{_endPoint}/login", loginRequest);
        //    var contentString = await result.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<LoginResponse>(contentString)!;
        //}

        //public async Task<LogoutResponse> RequestLogoutAsync()
        //{
        //    var result = await _httpClient.GetAsync($"{_endPoint}/logout");
        //    var contentString = await result.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<LogoutResponse>(contentString)!;
        //}

        //public async Task<SignUpResponse> RequestSignUpAsync(SignUpRequest signUpRequest)
        //{
        //    var result = await _httpClient.PostAsJsonAsync($"{_endPoint}/signup", signUpRequest);
        //    var contentString = await result.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<SignUpResponse>(contentString)!;
        //}
    }
}
