using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Haxgo.Core.Security;
using Haxgo.Data;
using Microsoft.Practices.Unity;

namespace Haxgo.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ExecuteStartupTasks();
        }

        protected void ExecuteStartupTasks()
        {
            IUnityContainer container = new UnityContainer();
            ContainerBootstrapper.RegisterTypes(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            InitialAdminAccount();
        }

        private void InitialAdminAccount()
        {
            IRepository<Entities.User> ubll = (IRepository<Entities.User>)DependencyResolver.Current.GetService(typeof(IRepository<Entities.User>));
            if (!ubll.Table.Any(o => o.Name == "wengli"))
            {
                IEncryptionService bll = (IEncryptionService)DependencyResolver.Current.GetService(typeof(IEncryptionService));
                Entities.User user = new Entities.User();
                user.Id = Guid.NewGuid();
                user.Name = "wengli";
                user.PassWord = bll.EncryptText("123");
                ubll.Create(user);
            }
        }
    }
}