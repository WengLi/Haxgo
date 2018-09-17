using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Haxgo.Core;
using Haxgo.Core.Caching;
using Haxgo.Core.Security;
using Haxgo.Data;
using Haxgo.Entities;
using Haxgo.Web.Extensions;
using Haxgo.Web.Helper;
using Microsoft.Practices.Unity;

namespace Haxgo.Web.Controllers
{
    public class HomeController : Controller
    {
        [Dependency]
        public IRepository<Menu> MenuBLL { get; set; }
        [Dependency]
        public IRepository<Site> SiteBLL { get; set; }
        [Dependency]
        public IRepository<Category> CateBLL { get; set; }
        private List<Menu> MenuList
        {
            get
            {
                List<Menu> obj = CacheManager.Get(Consts.MenuCacheKey) as List<Menu>;
                if (obj != null)
                    return obj;
                else
                {
                    obj = MenuBLL.Table.ToList();
                    CacheManager.Insert(Consts.MenuCacheKey, obj);
                    return obj;
                }
            }
        }
        private List<Category> CategoryList
        {
            get
            {
                List<Category> obj = CacheManager.Get(Consts.CategoryCacheKey) as List<Category>;
                if (obj != null)
                    return obj;
                else
                {
                    obj = CateBLL.Table.ToList();
                    CacheManager.Insert(Consts.CategoryCacheKey, obj);
                    return obj;
                }
            }
        }
        private List<Site> SiteList
        {
            get
            {
                List<Site> obj = CacheManager.Get(Consts.SiteCacheKey) as List<Site>;
                if (obj != null)
                    return obj;
                else
                {
                    obj = SiteBLL.Table.ToList();
                    CacheManager.Insert(Consts.SiteCacheKey, obj);
                    return obj;
                }
            }
        }

        public ActionResult Index()
        {
            ViewBag.Menus = MenuList;
            ViewBag.Categorys= CategoryList;
            ViewBag.Sites = SiteList;
            return View();
        }
    }
}
