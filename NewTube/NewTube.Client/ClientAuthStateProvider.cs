using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using NewTube.Client.Clients;
using NewTube.Client.Models;
using System.Text;
using NewTube.Shared.Interfaces;

namespace NewTube.Client
{
    public class ClientAuthStateProvider : AuthenticationStateProvider, IAuthService
    {
        // TODO: I am fully aware that commenting-out code is bad. This is
        // purely done to make implementation of the IAuthService and
        // authentication system easier in the long-term. The return statements
        // will be implemented once the base of the authentication system is
        // functioning
        

        /// <summary>
        /// Map the JavaScript-formatted properties to C#-formatted classes.
        /// </summary>
        private readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        private readonly AuthClient AuthClient;
        private readonly HttpClient HttpClient;
        private readonly ILogger Logger;

        public ClientAuthStateProvider(AuthClient authClient, HttpClient httpClient, ILogger<ClientAuthStateProvider> logger)
        {
            this.AuthClient = authClient;
            this.HttpClient = httpClient;
            this.Logger = logger;
        }

        private bool IsAuthenticated { get; set; } = false;
        private readonly ClaimsPrincipal Unauthenticated = new(new ClaimsIdentity());

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="username">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The result serialized to a <see cref="FormResult"/></returns>
        public async Task RequestSignUpAsync(string username, string password)
        {
            string[] defaultDetail = ["An unknown error prevented registration from succeeding."];

            try
            {
                var result = await HttpClient.PostAsJsonAsync(
                    "resgister",
                    new { username, password }
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
        public async Task RequestLoginAsync(string email, string password)
        {
            try
            {
                // login with cookies
                var result = await HttpClient.PostAsJsonAsync(
                    "login?useCookies=true",
                    new { email, password }
                );

                if (result.IsSuccessStatusCode)
                {
                    // need to refresh auth state
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

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

        /// <summary>
        /// Get authentication state.
        /// </summary>
        /// <remarks>
        /// Called by Blazor anytime and authentication-based decision needs to
        /// be made, then cached until the changed state notification is raised.
        /// </remarks>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            IsAuthenticated = false;

            // default to not authenticated
            var user = Unauthenticated;

            try
            {
                // the user info endpoint is secured, so if the user isn't loggedin this will fail
                var userResponse = await HttpClient.GetAsync("manage/info");

                // throw if user info wasn't retrieved
                userResponse.EnsureSuccessStatusCode();

                // user is authenticated, so let's build their authenticated identity
                var userJson = await userResponse.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson, JsonSerializerOptions);

                if (userInfo != null)
                {
                    // in this example app, name and email are the same
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userInfo.Email),
                        new Claim(ClaimTypes.Email, userInfo.Email)
                    };

                    // add any additional claims
                    claims.AddRange(
                        userInfo.Claims.Where(
                            c => c.Key != ClaimTypes.Name && !string.IsNullOrEmpty(c.Value)
                        ).Select(
                            c => new Claim(c.Key, c.Value)
                        )
                    );

                    // request the roles endpoint for the user's roles
                    var rolesResponse = await HttpClient.GetAsync("roles");

                    // throw if request fails
                    rolesResponse.EnsureSuccessStatusCode();

                    // read th response into a string
                    var rolesJson = await rolesResponse.Content.ReadAsStringAsync();

                    // deserialize the roles string into an array
                    var roles = JsonSerializer.Deserialize<RoleClaim[]>(rolesJson, JsonSerializerOptions);

                    // add any roles to the claims collection
                    if (roles?.Length > 0)
                    {
                        foreach (var role in roles)
                        {
                            if (!string.IsNullOrEmpty(role.Type) && !string.IsNullOrEmpty(role.Value))
                            {
                                claims.Add(new Claim(role.Type, role.Value, role.ValueType, role.Issuer, role.OriginalIssuer));
                            }
                        }
                    }

                    // set the principal
                    var id = new ClaimsIdentity(claims, nameof(ClientAuthStateProvider));
                    user = new ClaimsPrincipal(id);
                    IsAuthenticated = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "App error");
            }

            // return the state
            return new AuthenticationState(user);
        }

        public async Task RequestLogoutAsync()
        {
            var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
            await HttpClient.PostAsync("logout", emptyContent);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
