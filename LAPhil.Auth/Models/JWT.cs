using System;
using System.Text;
using Newtonsoft.Json;


namespace LAPhil.Auth
{
    public class JWT
    {

        public JWTPayload Payload { get; set; }
        public JWTHeader Header { get; set; }
        public string Signature { get; set; }
        public string TokenString { get; set; }
        public string RefreshToken { get; set; }

        public bool IsExpired
        {
            get
            {
                var result = Payload.Exp < DateTimeOffset.Now.ToUnixTimeSeconds();
                return result;
            }
        }

        public static string PaddedBase64String(string value)
        {
            while(value.Length % 4 != 0)
            {
                value = $"{value}=";
            }

            return value;
        }
        public static JWT FromTokenString(string value)
        {
            var parts = value.Split('.');

            if (parts.Length != 3)
            {
                throw new ArgumentException($"JWT is not decomposable to 3 parts.");
            }

            var x = PaddedBase64String(parts[1]);
            var y = PaddedBase64String(parts[0]);
            var headerBytes = Convert.FromBase64String(PaddedBase64String(parts[0]));
            var headerJson = Encoding.UTF8.GetString(headerBytes);

            var payloadBytes = Convert.FromBase64String(PaddedBase64String(parts[1]));
            var payloadJson = Encoding.UTF8.GetString(payloadBytes);


            var header = JsonConvert.DeserializeObject<JWTHeader>(headerJson);
            var payload = JsonConvert.DeserializeObject<JWTPayload>(payloadJson);

            var result = new JWT
            {
                Header = header,
                Payload = payload,
                Signature = parts[2],
                TokenString = value
            };

            return result;

        }

        public JWT()
        {

        }
    }
}
