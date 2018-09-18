using System;


namespace LAPhil.Application
{
    public class ServiceResolutionException: Exception
    {
        public Exception Exception { get; private set; }

        public ServiceResolutionException(Exception exception): base(exception.Message)
        {
            Exception = exception;
        }


    }
}
