using System;
using System.Collections.Generic;

namespace Domion.Web.Helpers
{
    public class PagingCalculator
    {
        private const int DefaultPageSize = 10;

        public PagingCalculator(int itemCount, int? pageNumber, int? pageSize)
        {
            if (itemCount < 0) throw new ArgumentOutOfRangeException(nameof(itemCount), "Must be > 0");

            ItemCount = itemCount;

            PageSize = pageSize ?? DefaultPageSize;
            PageSize = PageSize < 1 ? DefaultPageSize : PageSize;
            
            PageCount = ItemCount / PageSize + (ItemCount % PageSize > 0 ? 1 : 0);

            Page = pageNumber ?? 1;

            if (pageNumber < 0 | PageCount == 0)
            {
                Page = 1;
            }

            if (PageCount > 0 & Page > PageCount)
            {
                Page = PageCount;
            }

            OutOfRange = (pageNumber.HasValue & Page != pageNumber) | (pageSize.HasValue & PageSize != pageSize);
            
            PagingValues = new Dictionary<string, object>();

            if (pageNumber.HasValue)
            {
                PagingValues.Add("p", Page);
            }

            if (pageSize.HasValue & PageSize != DefaultPageSize)
            {
                PagingValues.Add("ps", PageSize);
            }
        }

        public Dictionary<string, object> PagingValues { get; }
        
        public int ItemCount { get; }

        public bool OutOfRange { get; }

        public int Page { get; }

        public int PageCount { get; }

        public int PageSize { get; }

        public int Skip => (Page - 1) * PageSize;

        public int Take => PageSize;
    }
}
