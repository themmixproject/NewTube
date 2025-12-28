using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewTube.Shared.DataTransfer;

namespace NewTube.Server.Controllers;

[Route("api/_auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ClaimsPrincipal _claimsPrincipal;
    public AuthenticationController(ClaimsPrincipal claimsPrincipal) { 
        _claimsPrincipal = claimsPrincipal;
    }

    [Authorize]
    [HttpGet("state")]
    public UserAuthenticationStateResponse GetUserState()
    {
        var response = new UserAuthenticationStateResponse
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
            UserName = User.Identity?.Name ?? "",
            Claims = User.Claims
        }
        return Ok()
    }
}
