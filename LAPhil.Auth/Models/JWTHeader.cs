using System;


namespace LAPhil.Auth
{
    public class JWTHeader
    {
        public string Typ { get; set; }   
        public string Alg { get; set; }   

        public JWTHeader()
        {
        }
    }
}
