using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DFlow.Tenants.Core.Model;
using DFlow.WebApp.Models;
using DFlow.WebApp.Services;
using Domion.WebApp.Helpers;

namespace DFlow.WebApp.Features.Tenants
{
    public class TenantsController : Controller
    {
        private readonly TenantServices AppServices;

        public TenantsController(TenantServices appServices)
        {
            AppServices = appServices;
        }

        // GET: Tenants
        public async Task<IActionResult> Index(int? p, int? ps)
        {
            var viewModel = new TenantListViewModel();

            IQueryable<Tenant> query = AppServices.Query();

            var pager = new PagingCalculator(query.Count(), p, ps);

            if (pager.OutOfRange)
            {
                return RedirectToAction("Index", new { p = pager.Page, ps = pager.PageSize });
            }

            viewModel.Paging.CurrentPage = pager.Page;
            viewModel.Paging.ItemsPerPage = pager.PageSize;
            viewModel.Paging.TotalItems = pager.ItemCount;

            return View(viewModel);
        }

        // GET: Tenants/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tenant = await _context.Tenant
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (tenant == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tenant);
        //}

        // GET: Tenants/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Tenants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Owner,RowVersion")] Tenant tenant)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(tenant);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(tenant);
        //}

        // GET: Tenants/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tenant = await _context.Tenant.SingleOrDefaultAsync(m => m.Id == id);
        //    if (tenant == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(tenant);
        //}

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Owner,RowVersion")] Tenant tenant)
        //{
        //    if (id != tenant.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(tenant);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TenantExists(tenant.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View(tenant);
        //}

        // GET: Tenants/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tenant = await _context.Tenant
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (tenant == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tenant);
        //}

        // POST: Tenants/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var tenant = await _context.Tenant.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Tenant.Remove(tenant);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //private bool TenantExists(int id)
        //{
        //    return _context.Tenant.Any(e => e.Id == id);
        //}
    }
}
