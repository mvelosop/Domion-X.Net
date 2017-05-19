using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domion.FluentAssertions.Helpers
{
    public abstract class BaseDbSetup<TDbContext> where TDbContext: DbContext
    {
        public abstract TDbContext GetDbContext();

        protected virtual void SetupDatabase()
        {
            using (var dbContext = GetDbContext())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
