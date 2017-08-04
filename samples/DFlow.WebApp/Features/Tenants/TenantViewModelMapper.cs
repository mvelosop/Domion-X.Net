using DFlow.Tenants.Core.Model;
using Domion.WebApp.ViewModels;
using System;

namespace DFlow.WebApp.Features.Tenants
{
    public class TenantViewModelMapper : IViewModelMapper<TenantViewModel, Tenant>
    {
        public Tenant CreateEntity(TenantViewModel vm)
        {
            var entity = new Tenant();

            UpdateEntityData(entity, vm);

            return entity;
        }

        public TenantViewModel CreateViewModel(Tenant entity)
        {
            var vm = new TenantViewModel
            {
                Id = entity.Id,
                Owner = entity.Owner,
                Notes = "Nota simulada en el ViewModel",
                RowVersion = entity.RowVersion
            };

            return vm;
        }

        public Tenant UpdateEntity(TenantViewModel vm, Tenant entity)
        {
            if (vm.RowVersion == null || vm.RowVersion.Length == 0)
                throw new InvalidOperationException($"{nameof(UpdateEntity)} requires a valid {nameof(vm.RowVersion)}.");

            entity.RowVersion = vm.RowVersion;

            UpdateEntityData(entity, vm);
            
            return entity;
        }

        private void UpdateEntityData(Tenant entity, TenantViewModel vm)
        {
            entity.Owner = vm.Owner;
        }
    }
}
