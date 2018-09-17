using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Haxgo.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
         
            routes.MapGenericPathRoute(
                name: "Menu",
                url: "{SeName}",
                defaults: new { controller = "Menu", action = "Index" },
                namespaces: new[] { "Haxgo.Web.Controllers" }
                );
            routes.MapGenericPathRoute(
                name: "Category",
                url: "{SeName}/{CategoryName}",
                defaults: new { controller = "Category", action = "Index" },
                namespaces: new[] { "Haxgo.Web.Controllers" }
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Haxgo.Web.Controllers" }
            );
        }
    }
}