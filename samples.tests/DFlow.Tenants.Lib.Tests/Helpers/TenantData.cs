using DFlow.Tenants.Core.Model;

namespace DFlow.Tenants.Lib.Tests.Helpers
{
    public class TenantData
    {
        public TenantData()
        {
        }

        public TenantData(string name)
        {
            Owner = name;
        }

        public TenantData(Tenant entity)
        {
            Owner = entity.Owner;
        }

        public string Owner { get; set; }

        public Tenant CreateEntity()
        {
            return UpdateEntity(new Tenant());
        }

        public Tenant UpdateEntity(Tenant entity)
        {
            entity.Owner = Owner;

            return entity;
        }
    }
}
