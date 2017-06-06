using Domion.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domion.Lib.Extensions
{
    public static class IQueryManagerExtensions
    {
        public static T First<T>(this IQueryManager<T> manager, Expression<Func<T, bool>> where) where T : class
        {
            return manager.Query(where).First();
        }

        public static T FirstOrDefault<T>(this IQueryManager<T> manager, Expression<Func<T, bool>> where) where T : class
        {
            return manager.Query(where).FirstOrDefault<T>();
        }

        public static IIncludableQueryable<T, TProperty> Include<T, TProperty>(this IQueryManager<T> manager, Expression<Func<T, TProperty>> includeExpression) where T : class
        {
            return manager.Query().Include(includeExpression);
        }

        public static T Single<T>(this IQueryManager<T> manager, Expression<Func<T, bool>> where) where T : class
        {
            return manager.Query(where).Single<T>();
        }

        public static T SingleOrDefault<T>(this IQueryManager<T> manager, Expression<Func<T, bool>> where) where T : class
        {
            return manager.Query(where).SingleOrDefault<T>();
        }
    }
}
