using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.BL.Pagination
{
    public class PagedList<T>
    {
        public int Total { get; set; }
        public ICollection<T> Items { get; set; }
    }
}
