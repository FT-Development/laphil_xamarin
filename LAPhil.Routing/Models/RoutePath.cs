using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace LAPhil.Routing
{
    public class RoutePath
    {
        public Regex Regex;
        public List<RouteParam> Params;
        public Route Route;
    }
}
