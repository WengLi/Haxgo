using System;
using System.Web.Mvc;

namespace Haxgo.Services
{
    public class ViewPageActivator : IViewPageActivator
    {
        public object Create(ControllerContext controllerContext, Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
