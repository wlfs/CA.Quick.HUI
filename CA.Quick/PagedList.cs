using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Quick
{

    public class PagedList<T> : List<T>
    {
        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagedList(IEnumerable<T> source=null, int totalCount=0, int pageIndex = 1, int pageSize = 20)
        {
            if (source == null || source.Count() < 1)
                throw new System.ArgumentNullException("source");

            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;
            if (TotalCount % pageSize > 0)
                TotalPages++;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source);
        }
    }
}
