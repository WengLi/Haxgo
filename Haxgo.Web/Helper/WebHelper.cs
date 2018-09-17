using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Haxgo.Core;
using Haxgo.Core.Sessions;
using Haxgo.Entities;

namespace Haxgo.Web.Helper
{
    public class WebHelper
    {
        public static HttpContext HttpContext
        {
            get { return HttpContext.Current; }
        }
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static User CurrentUser 
        {
            get
            { 
                return SessionService.GetObject(Consts.UserSessionKey) as User; 
            }
            set 
            {
                if (value == null)
                    SessionService.RemoveObject(Consts.UserSessionKey);
                else
                    SessionService.InsertObject(Consts.UserSessionKey, value);
            }
        }
        /// <summary>
        /// 生成页码url
        /// </summary>
        /// <param name="path">绝对url</param>
        /// <param name="query">查询字符串</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">记录数</param>
        /// <returns></returns>
        public static string GeneratePageUrl(string path, string query, int pageIndex, int pageSize)
        {
            if (!string.IsNullOrEmpty(query))
            {
                query = query.Replace("?", "");
                 Dictionary<string, string> dictionary = new Dictionary<string, string>();
                 foreach (string str3 in query.Split(new char[] { '&' }))
                 {
                     if (!string.IsNullOrEmpty(str3))
                     {
                         string[] strArray = str3.Split(new char[] { '=' });
                         if (strArray.Length == 2)
                         {
                             if (strArray[0] != "p" && strArray[0] != "s")
                                 dictionary[strArray[0]] = strArray[1];
                         }
                         else
                         {
                             dictionary[str3] = null;
                         }
                     }
                 }
                 StringBuilder builder = new StringBuilder();
                 foreach (string str5 in dictionary.Keys)
                 {
                     if (builder.Length > 0)
                     {
                         builder.Append("&");
                     }
                     builder.Append(str5);
                     if (dictionary[str5] != null)
                     {
                         builder.Append("=");
                         builder.Append(dictionary[str5]);
                     }
                 }
                 query = builder.ToString();
                 if (!string.IsNullOrEmpty(query))
                 {
                     query += ("&p=" + pageIndex + "&s=" + pageSize);
                 }
                 else
                     query = "p=" + pageIndex + "&s=" + pageSize;
            }
            else
                query = "p=" + pageIndex + "&s=" + pageSize;
            return path + (string.IsNullOrEmpty(query) ? "" : ("?" + query));
        }
        /// <summary>
        /// Get URL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        public static string GetUrlReferrer()
        {
            string referrerUrl = string.Empty;
            //URL referrer is null in some case (for example, in IE 8)
            if (HttpContext != null &&
                HttpContext.Request != null &&
                HttpContext.Request.UrlReferrer != null)
                referrerUrl = HttpContext.Request.UrlReferrer.PathAndQuery;

            return referrerUrl;
        }
        /// <summary>
        /// Get context IP address
        /// </summary>
        /// <returns>URL referrer</returns>
        public static string GetCurrentIpAddress()
        {
            if (HttpContext != null &&
                HttpContext.Request != null &&
                HttpContext.Request.UserHostAddress != null)
                return HttpContext.Request.UserHostAddress;
            return string.Empty;
        }
        /// <summary>
        /// Gets server variable by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Server variable</returns>
        public static string ServerVariables(string name)
        {
            string tmpS = string.Empty;
            try
            {
                if (HttpContext.Request.ServerVariables[name] != null)
                {
                    tmpS = HttpContext.Request.ServerVariables[name];
                }
            }
            catch
            {
                tmpS = string.Empty;
            }
            return tmpS;
        }
        /// <summary>
        /// Get Local Host Url
        /// </summary>
        /// <returns></returns>
        public static string GetLocalHost()
        {
            var httpHost = ServerVariables("HTTP_HOST");
            var result = "";
            if (!String.IsNullOrEmpty(httpHost))
            {
                result = "http://" + httpHost;
            }
            if (!result.EndsWith("/"))
                result += "/";
            return result.ToLowerInvariant();
        }
    }
}