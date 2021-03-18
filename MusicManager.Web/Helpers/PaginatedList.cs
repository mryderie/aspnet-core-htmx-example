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

        public PaginatedList(IList<T> items, int totalCount, int pageSize, int pageIndex, IDictionary<string, string> parameters)
        {
            PagingData = new PagingData(totalCount, pageSize, pageIndex, parameters);
            AddRange(items);
        }
    }
}
