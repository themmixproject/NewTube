using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using NewTube.Client.Clients;
using NewTube.Client.Models;

namespace NewTube.Client
{
    public class ClientAuthStateProvider : AuthenticationStateProvider
    {
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
        public async Task<FormResult> RegisterAsync(string username, string password)
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
                    return new FormResult { Succeeded = true };
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

                return new FormResult
                {
                    Succeeded = false,
                    ErrorList = problemDetails == null ? defaultDetail : [.. errors]
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "App error");
            }

            return new FormResult
            {
                Succeeded = false,
                ErrorList = defaultDetail
            };
         } 
    }
}
