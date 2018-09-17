using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Haxgo.Entities
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(250, ErrorMessage = "长度不能超过250")]
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(250, ErrorMessage = "长度不能超过250")]
        public string PassWord { get; set; }
    }
}
