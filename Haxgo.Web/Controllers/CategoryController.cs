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
    public class CategoryController : Controller
    {
        [Dependency]
        public IRepository<Category> CateBLL { get; set; }
        [Dependency]
        public IRepository<Menu> MenuBLL { get; set; }
        [Dependency]
        public IRepository<Site> SiteBLL { get; set; }
        [Dependency]
        public IRepository<UrlRecord> UrlBLL { get; set; }

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
        private List<Category> TableCache 
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

        public ActionResult Index(Guid mid, Guid cid)
        {
            Menu menu = MenuList.FirstOrDefault(o => o.Id == mid);
            Category cate = TableCache.FirstOrDefault(o => o.Id == cid);
            ViewBag.MenuId = mid;
            ViewBag.MenuName = menu.Name;
            ViewBag.SeName = menu.GetSeName();
            ViewBag.CategorySeName = cate.GetSeName();
            ViewBag.Categorys = TableCache;
            ViewBag.Category = cate;
            ViewBag.Sites = SiteList;
            return View();
        }

        [UserAuthorize]
        public ActionResult CategoryList()
        {
            return View();
        }
        [UserAuthorize]
        public ActionResult CategoryListJson(Guid? menuid)
        {
            List<Category> list = TableCache;
            if (menuid != null)
                list = list.Where(o => o.MenuId == menuid).OrderBy(o => o.ShowOrder).ToList();
            list.ForEach(o => { o.MenuName = MenuList.Where(m => m.Id == o.MenuId).Select(m => m.Name).FirstOrDefault(); });
            return Json(new DataGrid(list.Count, list), JsonRequestBehavior.AllowGet);
        }

        [UserAuthorize]
        public ActionResult Delete(Guid id)
        {
            foreach (Guid gid in GetIdAndChildId(TableCache.Where(o => o.Id == id).ToList()))
            {
                Category obj = CateBLL.GetById(gid);
                UrlRecord urlRecord = UrlBLL.Table.FirstOrDefault(o => o.EntityName == "Category" && o.EntityId == obj.Id);
                if (urlRecord != null)
                    UrlBLL.Delete(urlRecord);
                CateBLL.Delete(obj);
            }
            CacheManager.Remove(Consts.CategoryCacheKey);
            CacheManager.Remove(Consts.UrlRecordCacheKey);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public List<Guid> GetIdAndChildId(List<Category> query)
        {
            List<Guid> list = new List<Guid>();
            foreach (Category item in query)
            {
                list.Add(item.Id);
                if (TableCache.Any(o => o.ParentId == item.Id))
                {
                    list.AddRange(GetIdAndChildId(TableCache.Where(o => o.ParentId == item.Id).ToList()));
                }
            }
            return list;
        }
        [HttpPost]
        [UserAuthorize]
        public ActionResult Edit(Category obj)
        {
            if (string.IsNullOrEmpty(obj.Name) || obj.MenuId == Guid.Empty)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
            {
                Category model = CateBLL.GetById(obj.Id);
                if (model == null)
                    return Json(false, JsonRequestBehavior.AllowGet);
                else
                {
                    model.Name = obj.Name;
                    model.ShowOrder = obj.ShowOrder;
                    model.ParentId = obj.ParentId;
                    model.MenuId = obj.MenuId;
                    UrlRecord url = UrlBLL.Table.FirstOrDefault(o => o.EntityName == "Category" && o.EntityId == obj.Id);
                    url.Slug = obj.ValidateSeName("", obj.Name, true);
                    UrlBLL.Update(url);
                    CateBLL.Update(model);
                    CacheManager.Remove(Consts.CategoryCacheKey);
                    CacheManager.Remove(Consts.UrlRecordCacheKey);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        [UserAuthorize]
        public ActionResult Add(Category obj)
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
                    EntityName = "Category",
                    Slug = obj.ValidateSeName("", obj.Name, true)
                };
                UrlBLL.Create(url);
                CateBLL.Create(obj);
                CacheManager.Remove(Consts.CategoryCacheKey);
                CacheManager.Remove(Consts.UrlRecordCacheKey);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        [UserAuthorize]
        public ActionResult CategoryTreeJson()
        {
            List<EasyUITree> list = GetTreeJson(TableCache.Where(o => o.ParentId == null).ToList());
            list.Insert(0, new EasyUITree { });
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private List<EasyUITree> GetTreeJson(List<Category> query)
        {
            List<EasyUITree> list = new List<EasyUITree>();
            foreach (Category item in query)
            {
                EasyUITree obj = new EasyUITree();
                obj.id = item.Id;
                obj.text = item.Name;
                if (TableCache.Any(o => o.ParentId == item.Id))
                {
                    obj.children = GetTreeJson(TableCache.Where(o => o.ParentId == item.Id).ToList());
                }
                list.Add(obj);
            }
            return list;
        }
        #region 字符串递归(已注释)
        //private string GetTreeJson(List<Category> list)
        //{
        //    string content = "[";
        //    foreach (Category item in list)
        //    {
        //        content += "{";
        //        content += "\"id\":" + "\"" + item.Id + "\"";
        //        content += ",\"text\":" + "\"" + item.Name + "\"";
        //        if (TableCache.Any(o => o.ParentId == item.Id))
        //        {
        //            content += ",\"children\":" + GetTreeJson(TableCache.Where(o => o.ParentId == item.Id).ToList());
        //        }
        //        content += "},";
        //    }
        //    content = content.Substring(0, content.Length - 1) + "]";
        //    return content;
        //}
        #endregion
    }
}
