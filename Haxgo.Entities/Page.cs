using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haxgo.Entities
{
    public class Page
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages 
        {
            get 
            {
                int totalPages = TotalCount / PageSize;
                if (TotalCount % PageSize > 0)
                    totalPages++;
                return totalPages;
            }
        }
        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }
    }
}
