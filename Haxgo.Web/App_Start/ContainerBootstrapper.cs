using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Haxgo.Services;
using Haxgo.Data;
using Haxgo.Entities;
using Haxgo.Core.Security;

namespace Haxgo.Web
{
    public class ContainerBootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IEncryptionService, EncryptionService>()
                .RegisterType<IRepository<User>, EfRepository<User>>()
                .RegisterType<IRepository<Site>, EfRepository<Site>>()
                .RegisterType<IRepository<Category>, EfRepository<Category>>()
                .RegisterType<IRepository<Menu>, EfRepository<Menu>>()
                .RegisterType<IRepository<UrlRecord>, EfRepository<UrlRecord>>();
        }
    }
}