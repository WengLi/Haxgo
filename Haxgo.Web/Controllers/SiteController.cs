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
    public class SiteController : Controller
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

        private List<Site> TableCache
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
        [UserAuthorize]
        public ActionResult SiteList()
        {
            return View();
        }
        [UserAuthorize]
        public ActionResult SiteListJson(Guid? cateid, Guid? menuid, bool? ishome, bool? isbar, string name, int? page, int? rows)
        {
            IEnumerable<Site> query = TableCache;
            if (cateid != null)
                query = query.Where(o => o.CategoryId == cateid);
            if (menuid != null)
                query = query.Where(o => o.MenuId == menuid);
            if (ishome != null)
                query = query.Where(o => o.Is_Home == ishome);
            if (isbar != null)
                query = query.Where(o => o.Is_Bar == isbar);
            if (!string.IsNullOrEmpty(name))
                query = query.Where(o => o.Name.Contains(name));
            if (page == null)
                page = 1;
            if (rows == null)
                rows = 10;
            IEnumerable<Site> list = query.OrderBy(o => o.Id).Skip((page.Value - 1) * rows.Value).Take(rows.Value);
            foreach (Site item in list)
            {
                item.MenuName = MenuList.Where(o => o.Id == item.MenuId).Select(o => o.Name).FirstOrDefault();
                item.CategoryName = CategoryList.Where(o => o.Id == item.CategoryId).Select(o => o.Name).FirstOrDefault();
            }
            return Json(new DataGrid(query.Count(), list), JsonRequestBehavior.AllowGet);
        }
        [UserAuthorize]
        public ActionResult Delete(Guid id)
        {
            Site obj = SiteBLL.GetById(id);
            SiteBLL.Delete(obj);
            CacheManager.Remove(Consts.SiteCacheKey);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [UserAuthorize]
        public ActionResult Edit(Site obj)
        {
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj.Url) || obj.CategoryId == Guid.Empty)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
            {
                Site model = SiteBLL.GetById(obj.Id);
                if (model == null)
                    return Json(false, JsonRequestBehavior.AllowGet);
                else
                {
                    model.Name = obj.Name;
                    model.Url = obj.Url;
                    model.Description = obj.Description;
                    model.KeyWord = obj.KeyWord;
                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                        model.Logo = SaveFileReturnPath(Request.Files[0]);
                    model.MenuId = CategoryList.FirstOrDefault(o => o.Id == obj.CategoryId).MenuId;
                    model.CategoryId = obj.CategoryId;
                    model.Is_Home = obj.Is_Home;
                    model.Is_Bar = obj.Is_Bar;
                    model.ShowOrder = obj.ShowOrder;
                    SiteBLL.Update(model);
                    CacheManager.Remove(Consts.SiteCacheKey);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        [UserAuthorize]
        public ActionResult Add(Site obj)
        {
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj.Url) || (Request.Files.Count <= 0 || Request.Files[0].ContentLength <= 0) || obj.CategoryId == Guid.Empty)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
            {
                obj.Id = Guid.NewGuid();
                obj.Logo = SaveFileReturnPath(Request.Files[0]);
                obj.MenuId = CategoryList.FirstOrDefault(o => o.Id == obj.CategoryId).MenuId;
                SiteBLL.Create(obj);
                CacheManager.Remove(Consts.SiteCacheKey);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        private string SaveFileReturnPath(HttpPostedFileBase file)
        {
            string extion = file.FileName.Substring(file.FileName.LastIndexOf("."));
            string dic = Server.MapPath("/upload");
            if (!System.IO.Directory.Exists(dic))
                System.IO.Directory.CreateDirectory(dic);
            string path = "/upload/" + Guid.NewGuid().ToString().Replace("-", "") + extion;
            file.SaveAs(Server.MapPath(path));
            return path;
        }
    }
}
