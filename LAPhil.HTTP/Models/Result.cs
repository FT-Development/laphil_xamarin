using System;
using System.Threading.Tasks;


namespace LAPhil.HTTP
{
    public struct Result<TValue> 
    {
        public TValue Value { get; private set; }
        public Exception Error { get; private set; }
        public bool IsFailure => Error != null;
        public bool IsSuccess => Error == null;

        public Result(TValue value) 
        {
            Value = value;
            Error = null;
        }

        public Result(Exception ex) 
        {
            Value = default(TValue);
            Error = ex;   
        }

        public Result(Func<TValue> value)
        {
            try
            {
                Value = value();
                Error = null;
            }
            catch(Exception e)
            {
                Value = default(TValue);
                Error = e;
            }
        }


    }
}
