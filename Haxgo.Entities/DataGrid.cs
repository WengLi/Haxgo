using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haxgo.Entities
{
    public class DataGrid
    {
        public DataGrid() { }
        public DataGrid(int total, object rows)
        {
            this.rows = rows;
            this.total = total;
        }
        public DataGrid(int total, object rows, object footer)
        {
            this.rows = rows;
            this.total = total;
            this.footer = footer;
        }
        public int total { get; set; }
        public object rows { get; set; }
        public object footer { get; set; }
    }
}
