using BitzArt.Blazor.Auth;
using BitzArt.Blazor.Auth.Server;
using NewTube.Shared.DataTransfer;
using Microsoft.AspNetCore.Identity.Data;

namespace NewTube.Server;

public class AuthenticationService : AuthenticationService<Shared.DataTransfer.LoginRequest, Shared.DataTransfer.SignUpRequest>
{
    public override Task<AuthenticationResult> SignInAsync(Shared.DataTransfer.LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var jwtPair = BuildJwtPair();
        var authResult = Success(jwtPair);

        return Task.FromResult(authResult);
    }

    public override Task<AuthenticationResult> SignUpAsync(Shared.DataTransfer.SignUpRequest signUpRequest, CancellationToken cancellationToken = default)
    {
        var jwtPair = BuildJwtPair();
        var authResult = Success(jwtPair);

        return Task.FromResult(authResult);
    }

    public override Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var jwtPair = BuildJwtPair();
        var authResult = Success(jwtPair);

        return Task.FromResult(authResult);
    }

    private JwtPair BuildJwtPair()
    {
        return new JwtPair(
            "access-token-goes-here",
            DateTimeOffset.UtcNow.AddMinutes(15),
            "refresh-token-goes-here",
            DateTimeOffset.UtcNow.AddDays(7)
        );
    }
}
