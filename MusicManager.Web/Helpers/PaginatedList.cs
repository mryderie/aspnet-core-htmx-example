using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicManager.Web.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public PagingData PagingData { get; private set; }

        public PaginatedList(IList<T> items, int totalCount, int pageIndex, int pageSize, string currentSort = null, string currentFilter = null)
        {
            PagingData = new PagingData(totalCount, pageIndex, pageSize, currentSort, currentFilter);
            AddRange(items);
        }
    }
}
