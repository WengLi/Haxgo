using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Haxgo.Entities
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu : BaseEntity, ISlugSupported
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(250, ErrorMessage = "长度不能超过250")]
        public string Name { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int ShowOrder { get; set; }
    }
}
