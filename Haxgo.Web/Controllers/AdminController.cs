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
    public class AdminController : Controller
    {
        [Dependency]
        public IRepository<User> UserBLL { get; set; }
        [Dependency]
        public IEncryptionService EncryptionBLL { get; set; }
        [HttpGet]
        public ActionResult Login()
        {
            if (WebHelper.CurrentUser != null)
                return RedirectToAction("Index");
            else
                return View();
        }
        [HttpPost]
        public ActionResult Login(User obj)
        {
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj.PassWord))
                return View(obj);
            else
            {
                string pwd = EncryptionBLL.EncryptText(obj.PassWord);
                User model = UserBLL.Table.FirstOrDefault(o => o.Name == obj.Name && o.PassWord == pwd);
                if (model == null)
                    return View(obj);
                else
                {
                    WebHelper.CurrentUser = model;
                    if (!string.IsNullOrEmpty(Request["returnurl"]))
                        return Redirect(Request["returnurl"]);
                    else
                        return RedirectToAction("Index");
                }
            }
        }
        public ActionResult Logout()
        {
            WebHelper.CurrentUser = null;
            return RedirectToAction("Index", "Home");
        }
        [UserAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
