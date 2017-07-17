using cloudscribe.Web.Common.Extensions;
using DFlow.Tenants.Core.Model;
using DFlow.WebApp.Services;
using Domion.WebApp.Extensions;
using Domion.WebApp.Helpers;
using Domion.WebApp.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DFlow.WebApp.Features.Tenants
{
    public class TenantsController : Controller
    {
        public const string DbUpdateConcurrencyAlert = @"El cliente ""{0}"" fue modificado o eliminado por otro usuario, verifique los datos actualizados antes de intentarlo de nuevo.";
        public const string CreateSuccessAlert = @"Se agregó correctamente el cliente ""{0}"".";
        public const string DeleteSuccessAlert = @"Se eliminó correctamente el cliente ""{0}"".";
        public const string DeleteValidationAlert = @"No se pudo eliminar el cliente ""{0}"" por errores de validación, intentelo de nuevo, por favor.";
        public const string EntityNotFoundAlert = "No se pudo encontrar el cliente solicitado, pudo haber sido eliminado por otro usuario. (Id={0})";
        public const string UnexpectedErrorAlert = "Ocurrió un error inesperado! se creó un registro para investigar qué pasó.";
        public const string UpdateSuccessAlert = @"Se guardaron correctamente los cambios del cliente ""{0}"".";

        public const string ControllerExceptionLogMessage = "Controller Exception: {ex}";
        public const string EntityNotFoundLogMessage = "Could not find Tenant! (Id={id})";

        private readonly TenantsServices AppServices;
        private readonly ILogger<TenantsController> Logger;

        public TenantsController(ILogger<TenantsController> logger, TenantsServices appServices)
        {
            Logger = logger;
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
            Logger.LogInformation("Create: {@vm}", vm);

            if (ModelState.IsValid)
            {
                var entity = new Tenant();

                entity.Owner = vm.Owner;

                try
                {
                    var errors = AppServices.AddTenant(entity);

                    ModelState.ResetModelErrors(errors);

                    if (ModelState.IsValid)
                    {
                        AlertSuccess(CreateSuccessAlert, vm.Owner);

                        if (!errors.Any())
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(WebAppEvents.CREATE_POST, ex, ControllerExceptionLogMessage, ex);

                    AlertDanger(UnexpectedErrorAlert);
                }
            }

            return View(vm);
        }

        // GET: Tenants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var entity = FindTenantById(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var vm = new TenantViewModel();

                vm.Id = entity.Id;
                vm.Owner = entity.Owner;
                vm.Notes = "Nota simulada";
                vm.RowVersion = entity.RowVersion;

                var errors = AppServices.ValidateDelete(entity);

                ModelState.ResetModelErrors(errors);

                if (ModelState.IsValid)
                {
                    return View(vm);
                }

                return View("Details", vm);
            }
            catch (Exception ex)
            {
                Logger.LogError(WebAppEvents.DELETE_GET, ex, ControllerExceptionLogMessage, ex);

                AlertDanger(UnexpectedErrorAlert);
            }

            return RedirectToAction("Details", id);
        }

        // POST: Tenants/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, byte[] rowVersion)
        {
            var entity = FindTenantById(id);

            if (entity == null)
            {

                return RedirectToAction("Index");
            }

            try
            {
                entity.RowVersion = rowVersion;

                var errors = AppServices.DeleteTenant(entity);

                ModelState.ResetModelErrors(errors);

                if (ModelState.IsValid)
                {
                    AlertSuccess(DeleteSuccessAlert, entity.Owner);
                }
                else
                {
                    AlertWarning(DeleteValidationAlert, entity.Owner);

                    return RedirectToAction("Delete", new { id = entity.Id });
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                AlertDanger(DbUpdateConcurrencyAlert, entity.Owner);

                return RedirectToAction("Delete", new { id = entity.Id });
            }
            catch (Exception ex)
            {
                Logger.LogError(WebAppEvents.DELETE_POST, ex, ControllerExceptionLogMessage, ex);

                AlertDanger(UnexpectedErrorAlert);
            }

            return RedirectToAction("Index");
        }

        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var entity = FindTenantById(id);

            if (entity == null)
            {
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
            var entity = FindTenantById(id);

            if (entity == null)
            {
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
            var entity = FindTenantById(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }

            if (vm.Id != id)
            {
                Logger.LogWarning("id={id}, vm={mv}", id, vm);

                AlertWarning(UnexpectedErrorAlert);

                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    entity.Owner = vm.Owner;
                    entity.RowVersion = vm.RowVersion;

                    var errors = AppServices.UpdateTenant(entity);

                    ModelState.ResetModelErrors(errors);

                    if (ModelState.IsValid)
                    {
                        AlertSuccess(UpdateSuccessAlert, entity.Owner);

                        return RedirectToAction("Index");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    AlertDanger(DbUpdateConcurrencyAlert, entity.Owner);
                }
                catch (Exception ex)
                {
                    Logger.LogError(WebAppEvents.EDIT_POST, ex, ControllerExceptionLogMessage, ex);

                    AlertDanger(UnexpectedErrorAlert);
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

        private void AlertDanger(string message, params object[] args)
        {
            ControllerExtensions.AlertDanger(this, string.Format(message, args));
        }

        private void AlertSuccess(string message, params object[] args)
        {
            ControllerExtensions.AlertSuccess(this, string.Format(message, args));
        }

        private void AlertWarning(string message, params object[] args)
        {
            ControllerExtensions.AlertWarning(this, string.Format(message, args));
        }

        private Tenant FindTenantById(int? id)
        {
            var entity = AppServices.FindTenantById(id);

            if (entity == null)
            {
                Logger.LogWarning(EntityNotFoundLogMessage, id);

                AlertWarning(EntityNotFoundAlert, id);
            }

            return entity;
        }
    }
}
