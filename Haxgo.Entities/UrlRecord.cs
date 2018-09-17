using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Haxgo.Entities
{
    /// <summary>
    /// seo名称对照表
    /// </summary>
    public class UrlRecord : BaseEntity
    {
        /// <summary>
        /// 实体主键值
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// 实体名
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(250, ErrorMessage = "长度不能超过250")]
        public string EntityName { get; set; }

        /// <summary>
        /// seo名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(250, ErrorMessage = "长度不能超过250")]
        public string Slug { get; set; }
    }
}
