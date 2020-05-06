using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Webapp.Controllers
{
    public class VideoSourceTypeRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            var candidate = values[routeKey].ToString();
            return Enum.TryParse<VideoSourceType>(candidate, true, out VideoSourceType result);
        }
    }
}
