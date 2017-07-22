using cloudscribe.Web.Pagination;
using DFlow.Tenants.Core.Model;
using Domion.WebApp.Helpers;
using System.Collections.Generic;

namespace DFlow.WebApp.Features.Tenants
{
    public class TenantIndexViewModel
    {
        public TenantIndexViewModel()
        {
            Paging = new PaginationSettings();
        }

        public string Title { get; set; }     

        public string Search { get; set; }
        
        public List<Tenant> Items { get; set; }

        public PaginationSettings Paging { get; set; }

        public void SetPaging(PagingCalculator pager)
        {
            Paging.CurrentPage = pager.Page;
            Paging.ItemsPerPage = pager.PageSize;
            Paging.TotalItems = pager.ItemCount;
        }
    }
}
