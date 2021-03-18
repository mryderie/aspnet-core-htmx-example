using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicManager.Web.Helpers
{
    public class PagingData
    {
        public PagingData(int totalCount, int pageSize, int pageIndex, IDictionary<string, string> parameters)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            Parameters = parameters;
        }

        public string this[string paramName]
        {
            get
            {
                return Parameters.ContainsKey(paramName) ? Parameters[paramName] : null;
            }
        }

        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }

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

        public IDictionary<string, string> ParametersExcept(params string[] paramNames)
        {
            return Parameters.Where(p => !paramNames.Contains(p.Key))
                            .ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
