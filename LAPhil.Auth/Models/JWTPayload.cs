using System;


namespace LAPhil.Auth
{
    public class JWTPayload
    {
        public int Exp { get; set; }
        public string Session { get; set; }

        public JWTPayload()
        {
        }
    }
}
