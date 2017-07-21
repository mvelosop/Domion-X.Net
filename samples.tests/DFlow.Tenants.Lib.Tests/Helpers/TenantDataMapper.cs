using DFlow.Tenants.Core.Model;
using Domion.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return UpdateEntity(new Tenant(), data);
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

        public Tenant UpdateEntity(Tenant entity, TenantData data)
        {
            entity.Owner = data.Owner;

            return entity;
        }
    }
}
