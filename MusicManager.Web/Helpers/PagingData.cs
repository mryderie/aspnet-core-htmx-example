using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicManager.Web.Helpers
{
    public class PagingData
    {
        public PagingData(int totalCount, int pageIndex, int pageSize, string currentSort = null, string currentFilter = null)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            CurrentSort = currentSort;
            CurrentFilter = currentFilter;
        }

        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public string CurrentSort { get; private set; }
        public string CurrentFilter { get; private set; }


        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
    }
}
