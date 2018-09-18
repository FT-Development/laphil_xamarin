using System;
using System.Collections.Generic;


namespace LAPhil.Routing
{
    public class Request
    {
        public Dictionary<string, string> Params = new Dictionary<string, string>();
        public string Path;
    }
}
