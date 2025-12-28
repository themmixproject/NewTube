using System;
using System.Security.Claims;

namespace NewTube.Shared.DataTransfer;

public class UserAuthenticationStateResponse
{
    public bool IsAuthenticated { get; set; } = false;
    public string UserName { get; set; } = "";
    public Claim[] Claims { get; set; } = [];
}
