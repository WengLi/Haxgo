using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Haxgo.Core.Sessions
{
    public class SessionService
    {

        public static void InsertObject(string Key, object Obj)
        {
            if ((Obj != null))
            {
                if (HttpContext.Current.Session[Key] == null)
                {
                    HttpContext.Current.Session[Key] = Obj;
                }
            }
        }

        public static object GetObject(string Key)
        {
            return HttpContext.Current.Session[Key];
        }

        public static void RemoveObject(string Key)
        {
            if ((HttpContext.Current.Session[Key] != null))
            {
                HttpContext.Current.Session[Key] = null;
            }
        }
    }
}
