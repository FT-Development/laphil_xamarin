using System;
namespace LAPhil.Auth
{
    public class AuthException : Exception
    {
        public AuthException() : base() { }
        public AuthException(string message) : base(message: message) { }
    }

    public class InvalidUsernameOrPassword: AuthException
    {
        public InvalidUsernameOrPassword()
        {
        }
    }

    public class InvalidToken : AuthException
    {
        public InvalidToken()
        {
        }
    }
}
