using Domion.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domion.Lib.Data
{
    public static class IEntityManagerExtensions
    {
        public static IIncludableQueryable<T, TProperty> Include<T, TProperty>(this IEntityManager<T> manager, Expression<Func<T, TProperty>> includeExpression) where T : class
        {
            return manager.Query().Include(includeExpression);
        }
    }
}
