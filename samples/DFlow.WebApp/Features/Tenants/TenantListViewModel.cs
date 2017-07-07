using cloudscribe.Web.Pagination;
using DFlow.Tenants.Core.Model;
using System.Collections.Generic;

namespace DFlow.WebApp.Features.Tenants
{
    public class TenantListViewModel
    {
        public TenantListViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<Tenant> Items { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
