using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Haxgo.Entities
{
    /// <summary>
    /// 网站
    /// </summary>
    public class Site : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(250, ErrorMessage = "长度不能超过250")]
        public string Name { get; set; }
        /// <summary>
        /// 网址
        /// </summary>
        [Required(ErrorMessage = "网址不能为空")]
        [StringLength(250, ErrorMessage = "长度不能超过250")]
        public string Url { get; set; }
        /// <summary>
        ///  网站Logo
        /// </summary>
        [Required(ErrorMessage = "网站Logo不能为空")]
        [StringLength(250, ErrorMessage = "长度不能超过250")]
        public string Logo { get; set; }
        /// <summary>
        /// 所属类别
        /// </summary>
        public Guid CategoryId { get; set; }
        /// <summary>
        /// 所属类别名
        /// </summary>
        [NotMapped]
        public string CategoryName { get; set; }
        /// <summary>
        /// 所属菜单
        /// </summary>
        public Guid MenuId { get; set; }
        /// <summary>
        /// 所属菜单名
        /// </summary>
        [NotMapped]
        public string MenuName { get; set; }
        /// <summary>
        /// 是否首页推线显示
        /// </summary>
        public bool Is_Home { get; set; }
        /// <summary>
        /// 是否分类推荐显示
        /// </summary>
        public bool Is_Bar { get; set; }
        /// <summary>
        /// 网站描述
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int ShowOrder { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        [MaxLength(250)]
        public string KeyWord { get; set; }
    }
}
