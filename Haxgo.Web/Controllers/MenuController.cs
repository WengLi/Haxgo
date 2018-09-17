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
    public class MenuController : Controller
    {
        [Dependency]
        public IRepository<Menu> MenuBLL { get; set; }
        [Dependency]
        public IRepository<Site> SiteBLL { get; set; }
        [Dependency]
        public IRepository<Category> CateBLL { get; set; }
        [Dependency]
        public IRepository<UrlRecord> UrlBLL { get; set; }

        private List<Menu> TableCache
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
        private List<UrlRecord> UrlRecordList 
        {
            get
            {
                List<UrlRecord> obj = CacheManager.Get(Consts.UrlRecordCacheKey) as List<UrlRecord>;
                if (obj != null)
                    return obj;
                else
                {
                    obj = UrlBLL.Table.ToList();
                    CacheManager.Insert(Consts.UrlRecordCacheKey, obj);
                    return obj;
                }
            }
        }

        [UserAuthorize]
        public ActionResult MenuList()
        {
            return View();
        }
        [UserAuthorize]
        public ActionResult MenuListJson(int? page, int? rows)
        {
            if (page == null)
                page = 1;
            if (rows == null)
                rows = 10;
            var list = TableCache.OrderBy(o => o.ShowOrder).Skip((page.Value - 1) * rows.Value).Take(rows.Value);
            return Json(new DataGrid(TableCache.Count, list), JsonRequestBehavior.AllowGet);
        }
        [UserAuthorize]
        public ActionResult Delete(Guid id)
        {
            Menu obj = MenuBLL.GetById(id);
            UrlRecord urlRecord = UrlBLL.Table.FirstOrDefault(o => o.EntityName == "Menu" && o.EntityId == obj.Id);
            if (urlRecord != null)
                UrlBLL.Delete(urlRecord);
            MenuBLL.Delete(obj);         
            CacheManager.Remove(Consts.MenuCacheKey);
            CacheManager.Remove(Consts.UrlRecordCacheKey);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [UserAuthorize]
        public ActionResult Edit(Menu obj)
        {
            if (string.IsNullOrEmpty(obj.Name))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
            {
                Menu model = MenuBLL.GetById(obj.Id);
                if (model == null)
                    return Json(false, JsonRequestBehavior.AllowGet);
                else
                {
                    model.Name = obj.Name;
                    model.ShowOrder = obj.ShowOrder;
                    UrlRecord url = UrlBLL.Table.FirstOrDefault(o => o.EntityName == "Menu" && o.EntityId == obj.Id);
                    url.Slug = obj.ValidateSeName("", obj.Name, true);
                    UrlBLL.Update(url);
                    MenuBLL.Update(model);
                    CacheManager.Remove(Consts.MenuCacheKey);
                    CacheManager.Remove(Consts.UrlRecordCacheKey);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        [UserAuthorize]
        public ActionResult Add(Menu obj)
        {
            if (string.IsNullOrEmpty(obj.Name))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
            {
                obj.Id = Guid.NewGuid();
                UrlRecord url = new UrlRecord
                {
                    Id = Guid.NewGuid(),
                    EntityId = obj.Id,
                    EntityName = "Menu",
                    Slug = obj.ValidateSeName("", obj.Name, true)
                };
                UrlBLL.Create(url);
                MenuBLL.Create(obj);
                CacheManager.Remove(Consts.MenuCacheKey);
                CacheManager.Remove(Consts.UrlRecordCacheKey);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [UserAuthorize]
        public ActionResult MenuJson()
        {
            return Json(TableCache.OrderBy(o => o.ShowOrder), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(Guid id)
        {
            Menu menu = TableCache.FirstOrDefault(o => o.Id == id);
            ViewBag.MenuId = id;
            ViewBag.SeName = menu.GetSeName();
            ViewBag.MenuName = menu.Name;
            ViewBag.Categorys = CategoryList;
            ViewBag.Sites = SiteList;
            return View();
        }
    }
}
