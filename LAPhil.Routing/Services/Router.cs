using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace LAPhil.Routing
{
    public class Router
    {
        public List<Route> Children { get; private set; }
        public bool AppendTrailingSlash { get; private set; }
        public List<RoutePath> Paths { get; } = new List<RoutePath>();

        public Router(List<Route> children, bool appendTrailingSlash = true)
        {
            Children = children;
            Children.ForEach(x => x.Router = this);
            AppendTrailingSlash = appendTrailingSlash;
        }

        public void Navigate(string path)
        {
            if (path.EndsWith("/", StringComparison.InvariantCulture) == false && AppendTrailingSlash)
            {
                path = $"{path}/";
            }

            foreach (var routePath in Paths)
            {
                var match = routePath.Regex.Match(path);
                if (match.Success == false) continue;

                var requestParams = new Dictionary<string, string>();

                for (var i = 1; i < match.Groups.Count; i++)
                {
                    var value = match.Groups[i].Value;
                    var param = routePath.Params[i - 1];
                    requestParams[param.Name] = value;
                }

                var request = new Request
                {
                    Params = requestParams,
                    Path = path
                };

                routePath.Route.Action(request);
            }
        }

        public void Register(Route route)
        {
            string path = route.Path.Trim();
            RoutePath routePath;

            if (path.StartsWith("/", StringComparison.InvariantCulture))
            {
                path = path.Substring(1);
            }

            var segments = path.Split('/').ToList();

            var routeParams = new List<RouteParam>();
            var pathBuilder = new StringBuilder();

            segments.ForEach(x => {
                var segment = ParseSegment(x);

                pathBuilder.Append($"/{segment.Segment}");
                routeParams.AddRange(segment.Params);
            });

            routePath = new RoutePath
            {
                Regex = new Regex(pathBuilder.ToString(), RegexOptions.Compiled),
                Params = routeParams,
                Route = route
            };

            Paths.Add(routePath);
        }

        RouteSegment ParseSegment(string segment)
        {
            if (segment.Contains(':'))
            {
                return ParseParams(segment);
            }

            return new RouteSegment
            {
                Segment = segment
            };
        }

        RouteSegment ParseParams(string segment)
        {
            char[] delimiterChars = { '-', '.' };
            var defaultParamRegex = @".+";

            string segmentFormat = @"(?<{0}>{1})";
            StringBuilder path = new StringBuilder();
            StringBuilder name = new StringBuilder();
            StringBuilder regex = null;
            List<RouteParam> segmentParams = new List<RouteParam>();
            RouteParam param = new RouteParam();
            bool inRegex = false;

            foreach (char c in segment)
            {
                if (c == ':')
                {
                    if (name.Length > 0)
                    {
                        param.Name = name.ToString();
                        param.Regex = regex != null ? regex.ToString() : defaultParamRegex;

                        path.Append(String.Format(segmentFormat, param.Name, param.Regex));
                        segmentParams.Add(param);
                    }

                    name = new StringBuilder();
                    param = new RouteParam();
                    continue;
                }

                if (c == '(')
                {
                    regex = new StringBuilder();
                    inRegex = true;
                    continue;
                }

                if (c == ')')
                {
                    param.Name = name.ToString();
                    param.Regex = regex.ToString();
                    segmentParams.Add(param);

                    path.Append(String.Format(segmentFormat, param.Name, param.Regex));

                    name = new StringBuilder();
                    param = new RouteParam();

                    regex = null;
                    inRegex = false;
                    continue;
                }

                if (inRegex == false && delimiterChars.Contains(c))
                {
                    path.Append(c);
                    continue;
                }

                if (inRegex)
                {
                    regex.Append(c);
                }
                else
                {
                    name.Append(c);
                }
            }

            if (name.Length > 0)
            {
                param.Name = name.ToString();
                if (param.Regex == null)
                {
                    param.Regex = regex != null ? regex.ToString() : defaultParamRegex;
                }

                path.Append(String.Format(segmentFormat, param.Name, param.Regex));
                segmentParams.Add(param);
            }

            var finalPath = path.ToString();

            return new RouteSegment()
            {
                Params = segmentParams,
                Segment = finalPath
            };

        }
    }
}