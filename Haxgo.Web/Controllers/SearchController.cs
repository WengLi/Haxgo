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
    public class SearchController : Controller
    {
        [Dependency]
        public IRepository<Site> SiteBLL { get; set; }
        [Dependency]
        public IRepository<Category> CateBLL { get; set; }
        [Dependency]
        public IRepository<Menu> MenuBLL { get; set; }

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

        public ActionResult Index(string wd)
        {
            List<Site> list = new List<Site>();
            if (!string.IsNullOrEmpty(wd))
            {
                list = SiteList.Where(o => o.Name.Contains(wd) || o.KeyWord.Contains(wd)).ToList();
                list.ForEach(o =>
                {
                    o.MenuName = MenuList.Where(m => m.Id == o.MenuId).Select(m => m.Name).FirstOrDefault();
                    o.CategoryName = CategoryList.Where(m => m.Id == o.CategoryId).Select(m => m.Name).FirstOrDefault();
                });
            }
            ViewBag.KeyWord = wd;
            ViewBag.Count = list.Count;
            return View(list);
        }
    }
}
