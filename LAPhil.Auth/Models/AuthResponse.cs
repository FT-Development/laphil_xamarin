using System;
using System.Text;
using Newtonsoft.Json;

namespace LAPhil.Auth
{
    
    public class AuthResponse
    {
        public JWT Token { get; set; }
        public string RefreshToken { get; set; }

        public AuthResponse()
        {
        }
    }
}

