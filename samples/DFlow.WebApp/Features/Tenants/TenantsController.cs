using cloudscribe.Web.Common.Extensions;
using DFlow.Tenants.Core.Model;
using DFlow.WebApp.Services;
using Domion.WebApp.Extensions;
using Domion.WebApp.Helpers;
using Domion.WebApp.Logging;
using Domion.WebApp.Navigation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

namespace DFlow.WebApp.Features.Tenants
{
    public class TenantsController : Controller
    {
        public const string AddTitle = "Agregar Cliente";
        public const string ControllerExceptionLogMessage = "Controller Exception: {ex}";
        public const string CreateSuccessAlert = @"Se agregó correctamente el cliente ""{0}"".";
        public const string DbUpdateConcurrencyAlert = @"El cliente ""{0}"" fue modificado o eliminado por otro usuario, verifique los datos actualizados antes de intentarlo de nuevo.";
        public const string DeleteSuccessAlert = @"Se eliminó correctamente el cliente ""{0}"".";
        public const string DeleteTitle = "Eliminar Cliente";
        public const string DeleteValidationAlert = @"No se pudo eliminar el cliente ""{0}"" por errores de validación, intentelo de nuevo, por favor.";
        public const string DetailsTitle = "Consultar Cliente";
        public const string EditTitle = "Modificar Cliente";
        public const string EntityNotFoundAlert = "No se pudo encontrar el cliente solicitado, pudo haber sido eliminado por otro usuario. (Id={0})";
        public const string EntityNotFoundLogMessage = "Could not find Tenant! (Id={id})";
        public const string UnexpectedErrorAlert = "Ocurrió un error inesperado! se creó un registro para investigar qué pasó.";
        public const string UpdateSuccessAlert = @"Se guardaron correctamente los cambios del cliente ""{0}"".";

        private readonly TenantsServices _appServices;
        private readonly ILogger<TenantsController> _logger;

        public TenantsController(
            TenantsServices appServices,
            ILogger<TenantsController> logger)
        {
            _appServices = appServices;
            _logger = logger;

            LastIndexRouteDictionary = new Dictionary<string, string>();
            LastIndexRouteValues = new RouteValueDictionary();
        }

        public Dictionary<string, string> LastIndexRouteDictionary { get; }

        public RouteValueDictionary LastIndexRouteValues { get; private set; }

        // GET: Tenants/Create
        public IActionResult Create()
        {
            var vm = new TenantViewModel();

            SetupViewModel(vm, AddTitle);

            return View(vm);
        }

        // POST: Tenants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TenantViewModel vm)
        {
            _logger.LogInformation("Create: {@vm}", vm);

            if (ModelState.IsValid)
            {
                var entity = new Tenant();

                entity.Owner = vm.Owner;

                try
                {
                    var errors = _appServices.AddTenant(entity);

                    ModelState.ResetModelErrors(errors);

                    if (ModelState.IsValid)
                    {
                        this.AlertSuccess(CreateSuccessAlert, vm.Owner);

                        return RedirectToAction("Details", new { id = entity.Id });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(WebAppEvents.CREATE_POST, ex, ControllerExceptionLogMessage, ex);

                    this.AlertDanger(UnexpectedErrorAlert);
                }
            }

            SetupViewModel(vm, AddTitle);

            return View(vm);
        }

        // GET: Tenants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var entity = FindTenantById(id);

            if (entity == null)
            {
                return RedirectBackToIndex();
            }

            try
            {
                var vm = new TenantViewModel();

                vm.Id = entity.Id;
                vm.Owner = entity.Owner;
                vm.Notes = "Nota simulada";
                vm.RowVersion = entity.RowVersion;

                var errors = _appServices.ValidateDelete(entity);

                ModelState.ResetModelErrors(errors);

                SetupViewModel(vm, DeleteTitle);

                if (ModelState.IsValid)
                {
                    return View(vm);
                }

                return View("Details", vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(WebAppEvents.DELETE_GET, ex, ControllerExceptionLogMessage, ex);

                this.AlertDanger(UnexpectedErrorAlert);
            }

            return RedirectToAction("Details", new { id = id});
        }

        // POST: Tenants/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, byte[] rowVersion)
        {
            var entity = FindTenantById(id);

            if (entity == null)
            {
                return RedirectBackToIndex();
            }

            try
            {
                entity.RowVersion = rowVersion;

                var errors = _appServices.DeleteTenant(entity);

                ModelState.ResetModelErrors(errors);

                if (ModelState.IsValid)
                {
                    this.AlertSuccess(DeleteSuccessAlert, entity.Owner);
                }
                else
                {
                    this.AlertWarning(DeleteValidationAlert, entity.Owner);

                    return RedirectToAction("Delete", new { id = entity.Id });
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                this.AlertDanger(DbUpdateConcurrencyAlert, entity.Owner);

                return RedirectToAction("Delete", new { id = entity.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(WebAppEvents.DELETE_POST, ex, ControllerExceptionLogMessage, ex);

                this.AlertDanger(UnexpectedErrorAlert);
            }

            return RedirectBackToIndex();
        }

        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var entity = FindTenantById(id);

            if (entity == null)
            {
                return RedirectBackToIndex();
            }

            var vm = new TenantViewModel();

            vm.Id = entity.Id;
            vm.Owner = entity.Owner;
            vm.Notes = "Nota simulada";
            vm.RowVersion = entity.RowVersion;

            SetupViewModel(vm, DetailsTitle);

            return View(vm);
        }

        // GET: Tenants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var entity = FindTenantById(id);

            if (entity == null)
            {
                return RedirectBackToIndex();
            }

            var vm = new TenantViewModel();

            vm.Id = entity.Id;
            vm.Owner = entity.Owner;
            vm.Notes = "Nota simulada";
            vm.RowVersion = entity.RowVersion;

            SetupViewModel(vm, EditTitle);

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
                return RedirectBackToIndex();
            }

            if (vm.Id != id)
            {
                _logger.LogWarning("id={id}, vm={mv}", id, vm);

                this.AlertWarning(UnexpectedErrorAlert);

                return RedirectBackToIndex();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    entity.Owner = vm.Owner;
                    entity.RowVersion = vm.RowVersion;

                    var errors = _appServices.UpdateTenant(entity);

                    ModelState.ResetModelErrors(errors);

                    if (ModelState.IsValid)
                    {
                        this.AlertSuccess(UpdateSuccessAlert, entity.Owner);

                        return RedirectToAction("Details", new { id = entity.Id });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    this.AlertDanger(DbUpdateConcurrencyAlert, entity.Owner);
                }
                catch (Exception ex)
                {
                    _logger.LogError(WebAppEvents.EDIT_POST, ex, ControllerExceptionLogMessage, ex);

                    this.AlertDanger(UnexpectedErrorAlert);
                }
            }

            SetupViewModel(vm, EditTitle);

            return View(vm);
        }

        // GET: Tenants
        public async Task<IActionResult> Index(int? p, int? ps, string search)
        {
            this.SaveRouteValues();

            var vm = new TenantListViewModel();

            IQueryable<Tenant> query = _appServices.Search(search);

            // ReSharper disable once PossibleMultipleEnumeration
            var pager = new PagingCalculator(query.Count(), p, ps);

            if (pager.OutOfRange)
            {
                if (!string.IsNullOrWhiteSpace(search))
                {
                    pager.PagingValues.Add("search", search);
                }
                
                return RedirectToAction("Index", pager.PagingValues);
            }

            // ReSharper disable once PossibleMultipleEnumeration
            vm.Items = query
                .AsNoTracking()
                .OrderBy(t => t.Owner)
                .Skip(pager.Skip)
                .Take(pager.Take)
                .ToList();

            vm.SetPaging(pager);

            vm.Title = "Índice de Clientes";
            vm.Search = search;

            return View(vm);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var navHelper = new NavigationHelper(context);

            LastIndexRouteValues = navHelper.GetReturnRoute("Index");

            foreach (var item in LastIndexRouteValues)
            {
                LastIndexRouteDictionary.Add(item.Key, Convert.ToString(item.Value, CultureInfo.InvariantCulture));
            }
        }

        public IActionResult RedirectBackToIndex()
        {
            return RedirectToAction("Index", LastIndexRouteValues);
        }

        private Tenant FindTenantById(int? id)
        {
            var entity = _appServices.FindTenantById(id);

            if (entity == null)
            {
                _logger.LogWarning(EntityNotFoundLogMessage, id);

                this.AlertWarning(EntityNotFoundAlert, id);
            }

            return entity;
        }

        private void SetupViewModel(TenantViewModel vm, string title)
        {
            vm.Title = title;
            vm.LastIndexRouteDictionary = LastIndexRouteDictionary;
        }
    }
}
