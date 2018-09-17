using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haxgo.Entities
{
    public class EasyUITree
    {
        public object id { get; set; }
        public string text { get; set; }
        /// <summary>
        /// 合并则赋值closed，否则不赋值
        /// </summary>
        public string state { get; set; }
        public List<EasyUITree> children { get; set; }
    }
}
