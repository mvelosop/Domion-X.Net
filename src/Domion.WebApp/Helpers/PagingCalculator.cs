using System;

namespace Domion.WebApp.Helpers
{
    public class PagingCalculator
    {
        private const int DefaultPageSize = 10;

        public PagingCalculator(int itemCount, int? pageNumber, int? pageSize)
        {
            if (itemCount < 0) throw new ArgumentOutOfRangeException("Must be > 0", nameof(itemCount));

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
        }

        public int ItemCount { get; private set; }

        public bool OutOfRange { get; private set; }

        public int Page { get; private set; }

        public int PageCount { get; private set; }

        public int PageSize { get; private set; }

        public int Skip => (Page - 1) * PageSize;

        public int Take => PageSize;
    }
}
