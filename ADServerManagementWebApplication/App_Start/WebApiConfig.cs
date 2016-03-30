using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web;
using System.Web.Http.WebHost;
using System.Web.SessionState;
using System.Web.Routing;

namespace ADServerManagementWebApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}")
                .RouteHandler = new SessionStateRouteHandler(); 
        }

        public class SessionableControllerHandler : HttpControllerHandler, IRequiresSessionState
        {
            public SessionableControllerHandler(RouteData routeData)
                : base(routeData)
            { }
        }

        public class SessionStateRouteHandler : IRouteHandler
        {
            IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
            {
                return new SessionableControllerHandler(requestContext.RouteData);
            }
        }  
    }
}
