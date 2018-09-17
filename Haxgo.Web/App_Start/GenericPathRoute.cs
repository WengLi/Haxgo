using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Collections.Generic;
using Haxgo.Data;
using Haxgo.Entities;
using System.Web.Mvc;

namespace Haxgo.Web
{
    public class GenericPathRoute : Route
    {
        public GenericPathRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }
        public GenericPathRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }
        public GenericPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }
        public GenericPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData data = base.GetRouteData(httpContext);
            if (data != null)
            {
                IRepository<UrlRecord> ubll = (IRepository<UrlRecord>)DependencyResolver.Current.GetService(typeof(IRepository<UrlRecord>));
                var slug = data.Values["SeName"] as string;
                UrlRecord urlRecord = ubll.Table.Where(o => o.Slug == slug && o.EntityName == "Menu").FirstOrDefault();
                if (urlRecord == null)
                {
                    return null;
                }
                var subslug = data.Values["CategoryName"] as string;
                if (!string.IsNullOrWhiteSpace(subslug))
                {
                    UrlRecord SubUrlRecord = ubll.Table.Where(o => o.Slug == subslug && o.EntityName == "Category").FirstOrDefault();
                    if (SubUrlRecord == null)
                    {
                        return null;
                    }
                    data.Values["controller"] = "Category";
                    data.Values["action"] = "Index";
                    data.Values["mid"] = urlRecord.EntityId;
                    data.Values["cid"] = SubUrlRecord.EntityId;
                    data.Values["SeName"] = urlRecord.Slug;
                }
                else
                {
                    data.Values["controller"] = "Menu";
                    data.Values["action"] = "Index";
                    data.Values["id"] = urlRecord.EntityId;
                    data.Values["SeName"] = urlRecord.Slug;
                }
            }
            return data;
        }
    }
    public static class GenericPathRouteExtensions                      
    {
        //Override for localized route
        public static Route MapGenericPathRoute(this RouteCollection routes, string name, string url)
        {
            return MapGenericPathRoute(routes, name, url, null /* defaults */, (object)null /* constraints */);
        }
        public static Route MapGenericPathRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            return MapGenericPathRoute(routes, name, url, defaults, (object)null /* constraints */);
        }
        public static Route MapGenericPathRoute(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return MapGenericPathRoute(routes, name, url, defaults, constraints, null /* namespaces */);
        }
        public static Route MapGenericPathRoute(this RouteCollection routes, string name, string url, string[] namespaces)
        {
            return MapGenericPathRoute(routes, name, url, null /* defaults */, null /* constraints */, namespaces);
        }
        public static Route MapGenericPathRoute(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return MapGenericPathRoute(routes, name, url, defaults, null /* constraints */, namespaces);
        }
        public static Route MapGenericPathRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            var route = new GenericPathRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };

            if ((namespaces != null) && (namespaces.Length > 0))
            {
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return route;
        }
    }
}