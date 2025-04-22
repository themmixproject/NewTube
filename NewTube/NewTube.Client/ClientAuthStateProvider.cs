using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using NewTube.Client.Clients;
using NewTube.Client.Models;
using System.Text;
using NewTube.Shared.Interfaces;
using NewTube.Shared.DataTransfer;

namespace NewTube.Client
{
    public class ClientAuthStateProvider : AuthenticationStateProvider
    {
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
                    var rolesResponse = await HttpClient.GetAsync("auth/roles");

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

        /// <summary>
        /// An event that provides a notification when the <see cref="AuthenticationState"/>
        /// has changed. For example, this event may be raised if a user logs in or out.
        /// </summary>
        private new event AuthenticationStateChangedHandler? AuthenticationStateChanged;

        /// <summary>
        /// Raises the <see cref="AuthenticationStateChanged"/> event.
        /// </summary>
        /// <param name="task">A <see cref="Task"/> that supplies the updated <see cref="AuthenticationState"/>.</param>
        public new void NotifyAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            ArgumentNullException.ThrowIfNull(task);

            AuthenticationStateChanged?.Invoke(task);
        }

    }
}
