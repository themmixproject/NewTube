using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTube.Shared.DataTransfer
{
    public class SignUpRequest
    {
        public SignUpRequest() { }
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
