using System;
using BitzArt.Blazor.Auth;
using BitzArt.Blazor.Auth.Server;
using Microsoft.AspNetCore.Identity.Data;

namespace NewTube.Server;

public class AuthenticationService : AuthenticationService<LoginRequest>
{
    public override Task<AuthenticationResult> SignInAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
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
            DateTime.Now.AddDays(1),
            "refresh-token-goes-here",
            DateTime.Now.AddDays(1)
        );
    }
}
