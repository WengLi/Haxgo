using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Haxgo.Entities
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid Id { get; set; }
    }
}