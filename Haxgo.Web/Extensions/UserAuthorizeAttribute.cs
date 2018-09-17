using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Haxgo.Web.Helper;

namespace Haxgo.Web.Extensions
{
    /// <summary>
    /// 验证用户是否已登录
    /// </summary>
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (WebHelper.CurrentUser == null)
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Action", "Login" }, { "Controller", "Admin" }, { "returnurl", HttpContext.Current.Request.Url.ToString() } });
        }
    }
}