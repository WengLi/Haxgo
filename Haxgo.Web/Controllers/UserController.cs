using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Haxgo.Core.Security;
using Haxgo.Data;
using Haxgo.Entities;
using Haxgo.Web.Extensions;
using Haxgo.Web.Helper;
using Microsoft.Practices.Unity;

namespace Haxgo.Web.Controllers
{
    [UserAuthorize]
    public class UserController : Controller
    {
        [Dependency]
        public IRepository<User> UserBLL { get; set; }
        [Dependency]
        public IEncryptionService EncryptionBLL { get; set; }

        public ActionResult UserList(int? p, int? s)
        {
            return View();
        }

        public JsonResult UserListJson(int? page, int? rows)
        {
            if (page == null)
                page = 1;
            if (rows == null)
                rows = 10;
            var list = UserBLL.Table.OrderBy(o => o.Id).Skip((page.Value - 1) * rows.Value).Take(rows.Value);
            return Json(new DataGrid(UserBLL.Table.Count(), list), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(Guid id)
        {
            User obj = UserBLL.GetById(id);
            UserBLL.Delete(obj);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit(User obj)
        {
            User model = UserBLL.GetById(obj.Id);
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj.PassWord))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
            {
                string pwd = EncryptionBLL.EncryptText(obj.PassWord);
                if (model == null)
                    return Json(false, JsonRequestBehavior.AllowGet);
                else
                {
                    model.Name = obj.Name;
                    model.PassWord = pwd;
                    UserBLL.Update(model);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        public ActionResult Add(User obj)
        {
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj.PassWord))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
            {
                obj.PassWord = EncryptionBLL.EncryptText(obj.PassWord);
                obj.Id = Guid.NewGuid();
                UserBLL.Create(obj);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
