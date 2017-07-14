using cloudscribe.Web.Common.Extensions;
using DFlow.Tenants.Core.Model;
using DFlow.WebApp.Services;
using Domion.WebApp.Extensions;
using Domion.WebApp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

                try
                {
                    IEnumerable<ValidationResult> errors = AppServices.AddTenant(entity);

                    ModelState.ResetModelErrors(errors);

                    if (ModelState.IsValid)
                    {
                        this.AlertSuccess("Se guardó correctamente el nuevo cliente.");

                        if (!errors.Any())
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.AlertDanger("Ocurrió un error inesperado.");
                }
            }

            return View(vm);
        }

        // GET: Tenants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var entity = AppServices.FindTenantById(id);

            if (entity == null)
            {
                this.AlertWarning($"No se pudo encontrar el Cliente solicitado (Id={id}).");

                return RedirectToAction("Index");
            }

            try
            {
                IEnumerable<ValidationResult> errors = AppServices.ValidateDelete(entity);

                ModelState.ResetModelErrors(errors);

                if (ModelState.IsValid)
                {
                    var vm = new TenantViewModel();

                    vm.Id = entity.Id;
                    vm.Owner = entity.Owner;
                    vm.Notes = "Nota simulada";
                    vm.RowVersion = entity.RowVersion;

                    return View(vm);
                }

            }
            catch (Exception ex)
            {
                this.AlertDanger("Ocurrió un error inesperado.");
            }

            return RedirectToAction("Details", id);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var entity = AppServices.FindTenantById(id);

            if (entity == null)
            {
                this.AlertWarning($"No se pudo encontrar el Cliente solicitado (Id={id}).");

                return RedirectToAction("Index");
            }

            try
            {
                IEnumerable<ValidationResult> errors = AppServices.DeleteTenant(entity);
            }
            catch (Exception ex)
            {
                this.AlertDanger("Ocurrió un error inesperado.");
            }

            return RedirectToAction("Index");
        }

        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var entity = AppServices.FindTenantById(id);

            if (entity == null)
            {
                this.AlertWarning($"No se pudo encontrar el Cliente solicitado (Id={id}).");

                return RedirectToAction("Index");
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
            var entity = AppServices.FindTenantById(id);

            if (entity == null)
            {
                this.AlertWarning($"No se pudo encontrar el Cliente solicitado (Id={id}).");

                return RedirectToAction("Index");
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
        public async Task<IActionResult> Edit(int? id, TenantViewModel vm)
        {
            var entity = AppServices.FindTenantById(id);

            if (entity == null | vm.Id != id)
            {
                this.AlertWarning($"No se pudo encontrar el Cliente solicitado (Id={id}).");

                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    entity.Owner = vm.Owner;
                    entity.RowVersion = vm.RowVersion;

                    IEnumerable<ValidationResult> errors = AppServices.UpdateTenant(entity);

                    ModelState.ResetModelErrors(errors);

                    if (ModelState.IsValid)
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    this.AlertDanger($"El registro modificado o eliminado por otro usuario.");
                }
                catch (Exception ex)
                {
                    this.AlertDanger("Ocurrió un error inesperado.");
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
