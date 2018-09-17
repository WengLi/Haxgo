using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haxgo.Core
{
    public class Consts
    {
        /// <summary>
        /// 安全码
        /// </summary>
        public const string EncryptionPrivateKey = "Wengli_Haxgo_Nav_Site";

        /// <summary>
        ///  用户session名
        /// </summary>
        public const string UserSessionKey = "CurrentUserKey";

        /// <summary>
        /// 网站分类key
        /// </summary>
        public const string CategoryCacheKey = "CategoryCacheKey";

        /// <summary>
        /// 菜单key
        /// </summary>
        public const string MenuCacheKey = "MenuCacheKey";

        /// <summary>
        /// 站点key
        /// </summary>
        public const string SiteCacheKey = "SiteCacheKey";
        /// <summary>
        /// seo 网址
        /// </summary>
        public const string UrlRecordCacheKey = "UrlRecordCacheKey";
    }
}
