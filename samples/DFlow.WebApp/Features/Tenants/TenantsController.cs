using DFlow.Tenants.Core.Model;
using DFlow.WebApp.Services;
using Domion.WebApp.Extensions;
using Domion.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DFlow.WebApp.Features.Tenants
{
    public class TenantsController : Controller
    {
        private readonly TenantsServices AppServices;

        public TenantsController(TenantsServices appServices)
        {
            AppServices = appServices;
        }

        // GET: Tenants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tenants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TenantViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var entity = new Tenant();

                entity.Owner = vm.Owner;

                IEnumerable<ValidationResult> errors = AppServices.AddTenant(entity);

                return RedirectToAction("Index");
            }

            return View(vm);
        }

        // GET: Tenants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = AppServices.FindTenantById(id);

            if (entity == null)
            {
                return NotFound();
            }

            var vm = new TenantViewModel();

            vm.Id = entity.Id;
            vm.Owner = entity.Owner;
            vm.Notes = "Nota simulada";
            vm.RowVersion = entity.RowVersion;

            return View(vm);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = AppServices.FindTenantById(id);

            if (entity == null)
            {
                return NotFound();
            }

            IEnumerable<ValidationResult> errors = AppServices.DeleteTenant(entity);

            return RedirectToAction("Index");
        }

        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = AppServices.FindTenantById(id);

            if (entity == null)
            {
                return NotFound();
            }

            var vm = new TenantViewModel();

            vm.Id = entity.Id;
            vm.Owner = entity.Owner;
            vm.Notes = "Nota simulada";
            vm.RowVersion = entity.RowVersion;

            return View(vm);
        }

        // GET: Tenants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = AppServices.FindTenantById(id);

            if (entity == null)
            {
                return NotFound();
            }

            var vm = new TenantViewModel();

            vm.Id = entity.Id;
            vm.Owner = entity.Owner;
            vm.Notes = "Nota simulada";
            vm.RowVersion = entity.RowVersion;

            return View(vm);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TenantViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Tenant entity = AppServices.FindTenantById(vm.Id);

                    if (!EntityExists(vm.Id))
                    {
                        return NotFound();
                    }

                    entity.Owner = vm.Owner;

                    IEnumerable<ValidationResult> errors = AppServices.UpdateTenant(entity);

                    if (!errors.Any())
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.SetValidationResults(errors);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            return View(vm);
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

            viewModel.Items = query
                .AsNoTracking()
                .OrderBy(t => t.Owner)
                .Skip(pager.Skip)
                .Take(pager.Take)
                .ToList();

            viewModel.SetPaging(pager);

            return View(viewModel);
        }

        private bool EntityExists(int id)
        {
            return AppServices.FindTenantById(id) != null;
        }
    }
}
