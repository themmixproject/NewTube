namespace NewTube.Client
{
    // Add properties to this class and update the server and client AuthenticationStateProviders
    // to expose more information about the authenticated user to the client.

    /// <summary>
    /// User info from identity endpoint to establish claims.
    /// </summary>
    public class UserInfo
    {
        public required string UserId { get; set; }

        /// <summary>
        /// The email address.
        /// </summary>
        public required string Email { get; set; }

        public bool IsEmailConfigured { get; set; }

        /// <summary>
        /// The list of claims for the user.
        /// </summary>
        public Dictionary<string, string> Claims { get; set; } = [];
    }
}
