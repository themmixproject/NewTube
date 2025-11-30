using System;

namespace NewTube.Shared.DataTransfer;

public class LoginResponse
{
    public bool isSuccessful { get; set; }
    public string token { get; set; }
}
