using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Haxgo.Entities
{
    /// <summary>
    /// 类别名
    /// </summary>

    public class Category : BaseEntity, ISlugSupported
    {
        /// <summary>
        /// 类别名
        /// </summary>
        [Required(ErrorMessage = "类别名不能为空")]
        [StringLength(250, ErrorMessage = "长度不能超过250")]    
        public string Name { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int ShowOrder { get; set; }
        /// <summary>
        /// 父级类别
        /// </summary>
        public Guid? ParentId { get; set; }
        [NotMapped]
        public Guid? _parentId { get { return ParentId; } }
        /// <summary>
        /// 所属菜单
        /// </summary>
        public Guid MenuId { get; set; }
        /// <summary>
        /// 所属菜单名
        /// </summary>
        [NotMapped]
        public string MenuName { get; set; }
    }
}
