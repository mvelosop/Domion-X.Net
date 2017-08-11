using DFlow.Tenants.Core.Model;
using Domion.DataTools;

namespace DFlow.Tenants.Lib.Tests.Helpers
{
    public class TenantDataMapper : IDataMapper<TenantData, Tenant>
    {
        public TenantData CreateData(Tenant entity)
        {
            return new TenantData(entity.Owner);
        }

        public Tenant CreateEntity(TenantData data)
        {
            return UpdateEntity(data, new Tenant());
        }

        public Tenant DuplicateEntity(Tenant entity)
        {
            var duplicate = new Tenant
            {
                Id = entity.Id,
                Owner = entity.Owner,
                RowVersion = entity.RowVersion
            };

            return duplicate;
        }

        public Tenant UpdateEntity(TenantData data, Tenant entity)
        {
            entity.Owner = data.Owner;

            return entity;
        }
    }
}
