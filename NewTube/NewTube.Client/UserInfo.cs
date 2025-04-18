using System.Security.Claims;

namespace NewTube.Client
{
    // Add properties to this class and update the server and client AuthenticationStateProviders
    // to expose more information about the authenticated user to the client.
    public class UserInfo
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }

        public const string UserIdClaimType = "sub";
        public const string NameClaimType = "name";

        public static UserInfo FromClaimsPrincipal(ClaimsPrincipal principal)
        {
            return new UserInfo
            {
                UserId = GetRequiredClaim(principal, UserIdClaimType),
                Name = GetRequiredClaim(principal, NameClaimType),
                Email = ""
            };
        }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            return new ClaimsPrincipal(
                new ClaimsIdentity(
                    [
                        new Claim(UserIdClaimType, UserId),
                        new Claim(NameClaimType, Name)
                    ],
                    authenticationType: nameof(UserInfo),
                    nameType: NameClaimType,
                    roleType: null
                )
            );
        }

        public static string GetRequiredClaim(ClaimsPrincipal principal, string claimType)
        {
            return principal.FindFirst(claimType)?.Value ?? throw new InvalidOperationException($"Could not find reuired '{claimType}' claim.");
        }

    }
}
