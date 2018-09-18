using System;
using System.Collections.Generic;


namespace LAPhil.Routing
{
    public class Route
    {
        public List<Route> Children { get; private set; } = new List<Route>();
        public string Path { get; private set; }
        public Action<Request> Action { get; private set; }

        Router _router;
        public Router Router
        {
            get => _router;

            internal set
            {
                _router = value;
                Children.ForEach(x => x.Router = value);
                Router.Register(this);
            }
        }

        public Route(string path, Action<Request> action)
        {
            Path = path;
            Action = action;
        }

        public Route(string path, List<Route> children, Action<Request> action)
        {
            Path = path;
            Children = children;
            Action = action;
        }
    }
}
