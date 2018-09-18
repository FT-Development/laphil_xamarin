using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace LAPhil.HTTP
{
    public class RequestException : IOException
    {
        public RequestException() : base(){}
        public RequestException(string message) : base(message: message){}
    }

    public class TimeoutException : RequestException{}
    public class ConnectivityException : RequestException {};

    public class HTTPError : RequestException
    {
        public readonly HttpStatusCode Status;

        public HTTPError()
        {
        }

        public HTTPError(HttpStatusCode status)
        {
            Status = status;
        }

    }

    public class Unauthorized : HTTPError
    {
        public Unauthorized() : base(status: HttpStatusCode.Unauthorized)
        {

        }
    }

    public class Forbidden : HTTPError
    {
        public Forbidden() : base(status: HttpStatusCode.Forbidden)
        {

        }
    }

    public class NotFound : HTTPError
    {
        public NotFound() : base(status: HttpStatusCode.NotFound)
        {

        }
    }
}
