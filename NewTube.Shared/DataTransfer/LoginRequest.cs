using System;

namespace NewTube.Shared.DataTransfer;

public class LoginRequest
{
    public LoginRequest() { }

    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}
