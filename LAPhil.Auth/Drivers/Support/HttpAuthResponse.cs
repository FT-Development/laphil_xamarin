using System;
using Newtonsoft.Json;


namespace LAPhil.Auth
{
    public class HttpAuthResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
